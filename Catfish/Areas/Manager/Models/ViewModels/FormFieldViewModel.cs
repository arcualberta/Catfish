using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class FormFieldViewModel : KoBaseViewModel
    {
        public string TypeLabel { get; set; }
        public string FieldType { get; set; }
        public List<TextValue> Name { get; set; }
        //public string Description { get; set; }
        public List<TextValue> Description { get; set; }
        public bool IsRequired { get; set; }
        public bool IsOptionField { get; set; }
        public List<TextValue> MultilingualOptionSet { get; set; }
        public string Guid { get; set; }

        public FormFieldViewModel() { }
        // Attachment creates multiple recursions on view leaving the page unresponsive
        //[ScriptIgnore]
        //public Attachment Attachment { get; set; }
        //public List<FileViewModel> Files { get; set; }
        public string[] FieldFileGuids { get; set; }
        public List<FileViewModel> Files { get; set; }
        public int Rank { get; set; }
        public int Page { get; set; }
        public bool IsPageBreak { get; set; }

        private CatfishDbContext Db = new CatfishDbContext();

        public FormFieldViewModel(FormField src)
        {
            Name = src.MultilingualName.ToList();
            Description = src.MultilingualDescription.ToList();
            IsRequired = src.IsRequired;
            FieldType = src.GetType().AssemblyQualifiedName;
            Guid = src.Guid;
            Rank = src.Rank;
            Page = src.Page;
            IsPageBreak = src.IsPageBreak();
            Files = src.Files.Select( m => new FileViewModel(m, src.Id)).ToList();
            FieldFileGuids = src.FieldFileGuidsArray;
            //Files = src.Files;

            TypeLabelAttribute att = Attribute.GetCustomAttribute(src.GetType(), typeof(TypeLabelAttribute)) as TypeLabelAttribute;
            TypeLabel = att == null ? src.GetType().ToString() : att.Name;

            IsOptionField = typeof(OptionsField).IsAssignableFrom(src.GetType());
            if (IsOptionField)
            {
                MultilingualOptionSet = new List<TextValue>();

                //making sure we have an option-list editor for each language defined in the configuration settings.
                foreach(var lang in ConfigHelper.Languages)
                    MultilingualOptionSet.Add(new TextValue(lang.TwoLetterISOLanguageName, lang.NativeName, ""));

                IReadOnlyList<Option> options = (src as OptionsField).Options;
                foreach (Option op in options)
                {
                    foreach (TextValue txt in op.Value)
                    {
                        TextValue editorData = MultilingualOptionSet.Where(x => x.LanguageCode == txt.LanguageCode).FirstOrDefault();

                        //Accommodating odd situations where data has a language that is not specified in the configuration
                        if (editorData == null)
                            MultilingualOptionSet.Add(editorData = new TextValue(txt.LanguageCode, txt.LanguageLabel, ""));

                        if (string.IsNullOrEmpty(editorData.Value))
                            editorData.Value = txt.Value;
                        else
                            editorData.Value = editorData.Value + "\n" + txt.Value;
                    }
                }
            }
        }
        //XXX turns to database model
        public FormField InstantiateDataModel()
        {
            Type type = Type.GetType(FieldType, true);
            if (!typeof(FormField).IsAssignableFrom(type))
                throw new InvalidOperationException("Bad Type");

            FormField field = Activator.CreateInstance(type) as FormField;
            field.MultilingualName = Name;
            field.MultilingualDescription = Description;
            field.IsRequired = IsRequired;
            field.Guid = Guid;
            field.Rank = Rank;
            field.Page = Page;
            Files.Select(m => m.Guid);
            //XXX setter puts the <value element and sets value
            // this is where you search for the field on the database based on the fieldfileguids

            //XXX Quizas quita esto y toma los archivos de la lista de file elements
            //field.FieldFileGuids = String.Join("|", FieldFileGuids);
            field.FieldFileGuids = String.Join("|", Files.Select(m => m.Guid));

            UpdateFileList(field);

            if (typeof(OptionsField).IsAssignableFrom(type))
            {
                //Creating option list separately and assigning it to the Options propery of the Options field
                //to make sure that the overridden setter method is invoked to save data in XML
                List<Option> optList = new List<Option>();

                //In the this FormFieldViewModel, each TextValue element in the "MultilingualOptionSet" array
                //contains a text string of which each line represents an "option" in the data model.
                foreach (TextValue optionValueSet in MultilingualOptionSet)
                {
                    string[] optionTextValues = optionValueSet.Value
                        .Split(new char[] { '\r', '\n' }, StringSplitOptions.None)
                        .Select(v => v.Trim())
                        .Where(v => !string.IsNullOrEmpty(v))
                        .ToArray();

                    for (int i = 0; i < optionTextValues.Length; ++i)
                    {
                        if (optList.Count <= i)
                            optList.Add(new Option());

                        optList[i].Value.Add(new TextValue(optionValueSet.LanguageCode, optionValueSet.LanguageLabel, optionTextValues[i]));
                    }
                }

                (field as OptionsField).Options = optList;
            }

            return field;
        }

        ////protected List<Option> CreateOptions(string newLineSeparatedPptions)
        ////{
        ////    List<Option> optList = new List<Option>();
        ////    if (!string.IsNullOrEmpty(newLineSeparatedPptions))
        ////    {
        ////        string[] options = newLineSeparatedPptions.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        ////        foreach (string opt in options)
        ////            optList.Add(new Option(opt, false));
        ////    }
        ////    return optList;
        ////}

        public override void UpdateDataModel(object dataModel, CatfishDbContext db) { }

        private void UpdateFileList(FormField field)
        {
            // Remove old files (should we remove all files ?)

            List<string> test = field.FieldFileGuidsArray.ToList();

            foreach (DataFile file in field.Files.ToList())
            {
                if (test.IndexOf(file.Guid) < 0)
                {
                    //Deleting the file node from the XML Model
                    //XXX Missing remove file
                    //dstItem.RemoveFile(file);
                }
            }

            // Add new files
            //XXX Aqui es para recibir FieldFileGuids
            List<DataFile> filesList = new List<DataFile>();
            foreach (string fileGuid in field.FieldFileGuidsArray)
            {
                DataFile file = Db.XmlModels.Where(m => m.MappedGuid == fileGuid)
                    .Select(m => m as DataFile)
                    .FirstOrDefault();

                if (file != null)
                {
                    MoveFileToField(file, field);
                    filesList.Add(file);
                    Db.XmlModels.Remove(file);

                    // Move file from temp folder                    
                }
            }

            field.Files = filesList;
            Db.SaveChanges();            
        }

        //XXX Duplicating code from ItemService.cs UpdateFiles method

        private void MoveFileToField(DataFile file, FormField field)
        {
      
            //moving the physical files from the temporary upload folder to a folder identified by the GUID of the
            //item inside the uploaded data folder
            string dstDir = Path.Combine(ConfigHelper.DataRoot, field.MappedGuid);
            if (!Directory.Exists(dstDir))
                Directory.CreateDirectory(dstDir);

            string srcFile = Path.Combine(file.Path, file.LocalFileName);
            string dstFile = Path.Combine(dstDir, file.LocalFileName);
            File.Move(srcFile, dstFile);

            //moving the thumbnail, if it's not a shared one
            if (file.ThumbnailType == DataFile.eThumbnailTypes.NonShared)
            {
                string srcThumbnail = Path.Combine(file.Path, file.Thumbnail);
                string dstThumbnail = Path.Combine(dstDir, file.Thumbnail);
                File.Move(srcThumbnail, dstThumbnail);
            }

            //updating the file path
            file.Path = dstDir;
        }

    }
}
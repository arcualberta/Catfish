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
        public bool IsRichText { get; set; }
        public bool IsTextArea { get; set; }
        public bool IsOptionField { get; set; }
        public bool IsSliderField { get; set; }
        public List<TextValue> MultilingualOptionSet { get; set; }
        public string Guid { get; set; }

        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Step { get; set; }
        public string MinLabel { get; set; }
        public string MaxLabel { get; set; }

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

        public FormFieldViewModel(FormField formField, int abstractFormId)
        {
            Name = formField.MultilingualName.ToList();
            Description = formField.MultilingualDescription.ToList();
            IsRequired = formField.IsRequired;
            FieldType = formField.GetType().AssemblyQualifiedName;
            Guid = formField.Guid;
            Rank = formField.Rank;
            Page = formField.Page;
            IsPageBreak = formField.IsPageBreak();
            Files = formField.Files.Select( m => new FileViewModel(m, abstractFormId)).ToList();
            //FieldFileGuids = src.FieldFileGuidsArray;
            //Files = src.Files;

            CFTypeLabelAttribute att = Attribute.GetCustomAttribute(formField.GetType(), typeof(CFTypeLabelAttribute)) as CFTypeLabelAttribute;
            TypeLabel = att == null ? formField.GetType().ToString() : att.Name;

            IsSliderField = typeof(SliderField).IsAssignableFrom(formField.GetType());
            if (IsSliderField)
            {
                Min = ((SliderField)formField).Min;
                Max = ((SliderField)formField).Max;
                Step = ((SliderField)formField).Step;
                MinLabel = ((SliderField)formField).MinLabel;
                MaxLabel = ((SliderField)formField).MaxLabel;
            }


            IsOptionField = typeof(OptionsField).IsAssignableFrom(formField.GetType());
            if (IsOptionField)
            {
                MultilingualOptionSet = new List<TextValue>();

                //making sure we have an option-list editor for each language defined in the configuration settings.
                foreach (var lang in ConfigHelper.Languages)
                    MultilingualOptionSet.Add(new TextValue(lang.TwoLetterISOLanguageName, lang.NativeName, ""));

                IReadOnlyList<Option> options = (formField as OptionsField).Options;
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

            IsTextArea = typeof(TextArea).IsAssignableFrom(formField.GetType());
            if(IsTextArea){
                IsRichText = ((TextArea)formField).IsRichText;
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

            field.Files = Files != null ? Files.Select(m => m.ToFileDescription()).ToList() :
                 new List<CFFileDescription>();


            UpdateFileList(field);

            if (IsSliderField)
            {
                ((SliderField)field).Min = Min;
                ((SliderField)field).Max = Max;
                ((SliderField)field).Step = Step;
                ((SliderField)field).MinLabel = MinLabel;
                ((SliderField)field).MaxLabel = MaxLabel;
            }

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

            if (IsTextArea)
            {
                ((TextArea)field).IsRichText = IsRichText;
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

            List<CFDataFile> filesList = new List<CFDataFile>();
            foreach (CFFileDescription fileDescription in field.Files)
            {
                string fileGuid = fileDescription.Guid;
                CFDataFile file = Db.XmlModels.Where(m => m.MappedGuid == fileGuid)
                    .Select(m => m as CFDataFile)
                    .FirstOrDefault();

                if (file != null)
                {                     
                    MoveFileToField(file, field);
                    fileDescription.DataFile = file;
                    Db.XmlModels.Remove(file);                             
                }  
            }
            Db.SaveChanges();            
        }

        //XXX Duplicating code from ItemService.cs UpdateFiles method

        private void MoveFileToField(CFDataFile dataFile, FormField field)
        {

            //DataFile dataFile = fileDescription.DataFile;

            //moving the physical files from the temporary upload folder to a folder identified by the GUID of the
            //item inside the uploaded data folder
            string dstDir = Path.Combine(ConfigHelper.DataRoot, field.MappedGuid);
            if (!Directory.Exists(dstDir))
                Directory.CreateDirectory(dstDir);

            string srcFile = Path.Combine(dataFile.Path, dataFile.LocalFileName);
            string dstFile = Path.Combine(dstDir, dataFile.LocalFileName);
            File.Move(srcFile, dstFile);

            //moving the thumbnail, if it's not a shared one
            if (dataFile.ThumbnailType == CFDataFile.eThumbnailTypes.NonShared)
            {
                string srcThumbnail = Path.Combine(dataFile.Path, dataFile.Thumbnail);
                string dstThumbnail = Path.Combine(dstDir, dataFile.Thumbnail);
                File.Move(srcThumbnail, dstThumbnail);
            }

            //updating the file path
            dataFile.Path = dstDir;
        }

    }
}
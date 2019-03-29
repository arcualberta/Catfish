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
using static Catfish.Core.Models.Forms.CompositeFormField;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class ViewModelTuple<T1, T2>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public ViewModelTuple()
        {
            Item1 = default(T1);
            Item2 = default(T2);
        }

        public ViewModelTuple(T1 a, T2 b) : this()
        {
            Item1 = a;
            Item2 = b;
        }
    }
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
        public bool IsExternalMediaField { get; set; }
        public List<TextValue> MultilingualOptionSet { get; set; }
        public string Guid { get; set; }

        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public bool IsMultiple { get; set; } //Nov 19 2016 -- for multiple field entries
        public bool IsTextField { get; set; }
        public decimal Step { get; set; }
        public string MinLabel { get; set; }
        public string MaxLabel { get; set; }

        public string Source { get; set; }
        public string MimeType { get; set; }
        public List<KeyValuePair<string, ViewModelTuple<IEnumerable<string>, bool>>> MediaProperties{ get; set; }
        public FormFieldViewModel() {
            IsCompositeFormField = typeof(CompositeFormField).IsAssignableFrom(new FormFieldType().GetType());//March 27 2019
            Headers = new List<FormFieldViewModel>();
            HeaderSelectedFieldTypes = new List<FormFieldType>();
            
        }
        // Attachment creates multiple recursions on view leaving the page unresponsive
        //[ScriptIgnore]
        //public Attachment Attachment { get; set; }
        //public List<FileViewModel> Files { get; set; }
        public string[] FieldFileGuids { get; set; }
        public List<FileViewModel> Files { get; set; }
        public int Rank { get; set; }
        public int Page { get; set; }
        public bool IsPageBreak { get; set; }

        //adding extra field for CompositeFormField -- March 27 2019
        public bool IsCompositeFormField { get; set; }
        public List<FormFieldViewModel> Headers { get; set; }
        //public CompositeFormField CompositeFormField { get; set; }
        public bool Shuffle { get; set; }
        public eStepState StepState { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> SelectedStepState { get; set; }
        [NotMapped]
        public List<FormFieldType> HeaderSelectedFieldTypes { get; set; }
        [NotMapped]
        public  List<SelectListItem> StepSatesList
        {
            get
            {
                return Enum.GetNames(typeof(eStepState)).Select(e => new SelectListItem { Text = e, Value = ((int)Enum.Parse(typeof(eStepState), e)).ToString() }).ToList();  
            }
        }


        [NotMapped]
        public List<FormFieldType> FieldTypes
        {
            get
            {
                List<FormFieldType> mFieldTypes = new List<FormFieldType>();
                mFieldTypes.Add(new FormFieldType());

                var fieldTypes = typeof(FormField).Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(FormField))
                        && !t.CustomAttributes.Where(a => a.AttributeType.IsAssignableFrom(typeof(CFIgnoreAttribute))).Any())
                    .ToList();

                foreach (var t in fieldTypes)
                {
                    CFTypeLabelAttribute att = Attribute.GetCustomAttribute(t, typeof(CFTypeLabelAttribute)) as CFTypeLabelAttribute;

                    //We expect Metadata Fields that are usable by the interface
                    //to have a TypeLabel attribute to be defined (and labeled)
                    if (att != null)
                    {
                        mFieldTypes.Add(new FormFieldType()
                        {
                            FieldType = t.AssemblyQualifiedName,
                            Label = att.Name
                        });
                    }
                }
                return mFieldTypes;
            }
        }
        private void SetAttribute(string v, int value)
        {
            throw new NotImplementedException();
        }

        private eStepState GetAttribute(string v1, int v2)
        {
            throw new NotImplementedException();
        }

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

            IsExternalMediaField = typeof(ExternalMediaField).IsAssignableFrom(formField.GetType());
            MediaProperties = new List<KeyValuePair<string, ViewModelTuple<IEnumerable<string>, bool>>>();
            if (IsExternalMediaField)
            {
                Source = ((ExternalMediaField)formField).Source;
                MimeType = Enum.GetName(typeof(CFDataFile.MimeType), ((ExternalMediaField)formField).MediaType);

                var mediaTypeProperties = formField.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(MediaTypeAttribute)));

                foreach (var p in mediaTypeProperties)
                {
                    MediaProperties.Add(new KeyValuePair<string, ViewModelTuple<IEnumerable<string>, bool>>(p.Name, new ViewModelTuple<IEnumerable<string>, bool>(p.GetCustomAttribute<MediaTypeAttribute>(true)
                        .MimeTypes.Select(m => Enum.GetName(typeof(CFDataFile.MimeType), m)),
                        (bool)p.GetValue(formField))));
                }
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
            IsTextField = typeof(TextField).IsAssignableFrom(formField.GetType());
            if (IsTextField)
                IsMultiple = ((TextField)formField).IsMultiple; //Nov 19 2016

            IsCompositeFormField = typeof(CompositeFormField).IsAssignableFrom(formField.GetType());//March 27 2019

            if (SelectedStepState != null && SelectedStepState.Count() > 0)
            {
                StepState = (eStepState) Enum.Parse(typeof(eStepState), SelectedStepState.ElementAt(0).Text.ToString());
                
            }
           
        }

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

            if (IsExternalMediaField)
            {
                ((ExternalMediaField)field).Source = Source;
                ((ExternalMediaField)field).MediaType = (CFDataFile.MimeType)Enum.Parse(typeof(CFDataFile.MimeType), MimeType);

                var mediaTypeProperties = field.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(MediaTypeAttribute)));

                foreach(var entry in MediaProperties)
                {
                    mediaTypeProperties.Where(p => p.Name == entry.Key).First().SetValue(field, entry.Value.Item2);
                }
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

            if (IsTextField)
            {
                ((TextField)field).IsMultiple = IsMultiple;
            }

            IsCompositeFormField = typeof(CompositeFormField).IsAssignableFrom(field.GetType());//March 27 2019

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

        public override void UpdateDataModel(object dataModel, CatfishDbContext db) {

        }

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
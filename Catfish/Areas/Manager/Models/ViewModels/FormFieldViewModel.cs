using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public int Rank { get; set; }
        public int Page { get; set; }
        public bool IsPageBreak { get; set; }
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

    }
}
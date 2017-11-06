using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class MetadataFieldViewModel : KoBaseViewModel
    {
        public string TypeLabel { get; set; }
        public string FieldType { get; set; }
        public List<TextValue> Name { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public bool IsOptionField { get; set; }
        public List<TextValue> MultilingualOptionSet { get; set; }
        public string Ref { get; set; }

        public MetadataFieldViewModel() { }

        public MetadataFieldViewModel(MetadataField src)
        {
            Name = src.MultilingualName.ToList();

            Description = src.Description;
            IsRequired = src.IsRequired;
            FieldType = src.GetType().AssemblyQualifiedName;
            Ref = src.Ref;

            TypeLabelAttribute att = Attribute.GetCustomAttribute(src.GetType(), typeof(TypeLabelAttribute)) as TypeLabelAttribute;
            TypeLabel = att == null ? src.GetType().ToString() : att.Name;

            IsOptionField = typeof(OptionsField).IsAssignableFrom(src.GetType());
            if (IsOptionField)
            {
                MultilingualOptionSet = new List<TextValue>();

                //making sure we have an option-list editor for each language defined in the configuration settings.
                List<Language> languages = ConfigHelper.Languages;
                foreach(Language lang in languages)
                    MultilingualOptionSet.Add(new TextValue(lang.Code, lang.Label, ""));

                List<Option> options = (src as OptionsField).Options;
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

        public MetadataField InstantiateDataModel()
        {
            Type type = Type.GetType(FieldType, true);
            if (!typeof(MetadataField).IsAssignableFrom(type))
                throw new InvalidOperationException("Bad Type");

            MetadataField field = Activator.CreateInstance(type) as MetadataField;
            field.MultilingualName = Name;
            field.Description = Description;
            field.IsRequired = IsRequired;
            field.Ref = Ref;
            if (typeof(OptionsField).IsAssignableFrom(type))
            {
                //Creating option list separately and assigning it to the Options propery of the Options field
                //to make sure that the overridden setter method is invoked to save data in XML
                List<Option> optList = new List<Option>();

                //In the this MetadataFieldViewModel, each TextValue element in the "MultilingualOptionSet" array
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
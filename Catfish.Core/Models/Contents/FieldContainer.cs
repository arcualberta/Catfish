using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class FieldContainer : XmlModel
    {
        public const string FieldContainerTag = "fields";
        public Guid Id
        {
            get => Guid.Parse(Data.Attribute("id").Value);
            set => Data.SetAttributeValue("id", value);
        }
        public MultilingualName Name { get; protected set; }
        public MultilingualDescription Description { get; protected set; }
        public FieldList Fields { get; protected set; }

        public FieldContainer(string tagName) : base(tagName) { Initialize(eGuidOption.Ignore); }
        public FieldContainer(XElement data) : base(data) { Initialize(eGuidOption.Ignore); }
        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            Name = new MultilingualName(GetElement(MultilingualName.TagName, true));
            Description = new MultilingualDescription(GetElement(MultilingualDescription.TagName, true));
            Fields = new FieldList(GetElement(FieldContainerTag, true));
        }

        public string GetName(string lang)
        {
            if (string.IsNullOrEmpty(lang))
                return string.Join(" + ", Name.Values.Where(val => val.Value != null).Select(val => val.Value));
            else
            {
                Text val = Name.Values.Where(val => val.Language == lang).FirstOrDefault();
                return val != null ? val.Value : null;
            }
        }

        public void SetName(string containerName, string lang)
        {
            Name.SetContent(containerName, lang);
        }

        public string GetDescription(string lang)
        {
            Text val = Description.Values.Where(val => val.Language == lang).FirstOrDefault();
            return val != null ? val.Value : null;
        }

        public void SetDescription(string containerDescription, string lang)
        {
            Description.SetContent(containerDescription, lang);
        }

        /// <summary>
        /// Returns the Field of which the name is the name given by the input argument fieldName in the language specified by
        /// the input argument lang
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public BaseField GetFieldByName<T>(string fieldName, string lang)
            where T : BaseField
        {
            var fieldsOfGivenType = Fields.Where(f => f is T);
            foreach (var field in fieldsOfGivenType)
            {
                if (field.Name.Values.Where(v => v.Language == lang && v.Value == fieldName).FirstOrDefault() != null)
                    return field;
            }
            return null;
        }

        public T GetField<T>(string fieldName, string fieldNameLang, bool createIfNotExists = true)
            where T : TextField
        {
            T field = GetFieldByName<T>(fieldName, fieldNameLang) as T;

            if (field == null && createIfNotExists)
            {
                if (typeof(TextArea).IsAssignableFrom(typeof(T)))
                    field = new TextArea() as T;
                else
                    field = new TextField() as T;

                field.Name.SetContent(fieldName, fieldNameLang);
                Fields.Add(field);
            }

            return field;
        }


        public T CreateField<T>(string fieldName, string lang, bool? isRequired = null, bool? allowMultiple = null, object defaultValue = null)
            where T : MonolingualTextField
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);
            field.SetName(fieldName, lang);

            if (isRequired.HasValue)
                field.Required = isRequired.Value;

            if (allowMultiple.HasValue)
                field.AllowMultipleValues = allowMultiple.Value;

            if (defaultValue != null)
                field.SetValue(defaultValue);
            else
            {
                if (typeof(DateField).IsAssignableFrom(typeof(T)))
                    field.SetValue(new DateTime());
                else
                    field.SetValue(0);
            }

            return field;
        }
        public T CreateField<T>(string fieldName, string lang, bool? isRequired = null, bool? allowMultiple = null, string defaultValue = null)
            where T : TextField
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);
            field.SetName(fieldName, lang);

            if (isRequired.HasValue)
                field.Required = isRequired.Value;

            if (allowMultiple.HasValue)
                field.AllowMultipleValues = allowMultiple.Value;

            if (defaultValue != null)
                field.SetValue(defaultValue, lang);
            else
                field.SetValue("", lang);

            return field;
        }

        public void SetFieldValue<T>(string fieldName, string fieldNameLang, string fieldValue, string fieldValueLang, bool createIfNotExists = true, int valueIndex = 0) 
            where T : TextField
        {
            T field = GetField<T>(fieldName, fieldNameLang, createIfNotExists);
            field.SetValue(fieldValue, fieldValueLang, valueIndex);
        }
        public void SetFieldValue<T>(string fieldName, string fieldNameLang, string[] fieldValues, string fieldValueLang, bool createIfNotExists = true, int startValueIndex = 0)
            where T : TextField
        {
            T field = GetField<T>(fieldName, fieldNameLang, createIfNotExists);
            int valueIndex = startValueIndex;
            foreach (string val in fieldValues)
                field.SetValue(val, fieldValueLang, valueIndex);
        }

        public string GetValue<T>(string fieldName, string fieldNameLanguage, string fieldValueLanguage)
            where T : TextField
        {
            TextField field = GetField<T>(fieldName, fieldNameLanguage, false);
            return field != null ? field.GetValue(fieldValueLanguage) : null;
        }

        public List<string> GetValues<T>(string fieldName, string fieldNameLanguage, string fieldValueLanguage)
            where T : TextField
        {
            TextField field = GetField<T>(fieldName, fieldNameLanguage, false);
            List<string> values = new List<string>();
            foreach(var val in field.Values)
            {
                string v = val.Values.Where(txt => txt.Language == fieldValueLanguage).Select(txt => txt.Value).FirstOrDefault();
                if (!string.IsNullOrEmpty(v))
                    values.Add(v);
            }

            return values;
        }

        public T CreateField<T>(string fieldName, string lang, string[] optionTexts, bool? isRequired = null, int? defaultValueIndex = null)
            where T : OptionsField
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);
            field.SetName(fieldName, lang);
            field.AddOptions(optionTexts, lang, defaultValueIndex);

            if (isRequired.HasValue)
                field.Required = isRequired.Value;

            return field;
        }

        public T CreateField<T>(string content, string lang, string cssClass = null, string fieldName = null)
            where T : InfoSection
        {
            T field = Activator.CreateInstance(typeof(T)) as T;

            Fields.Add(field);

            field.SetContent(content, lang);

            if (!string.IsNullOrEmpty(fieldName))
                field.SetName(fieldName, lang);

            if (!string.IsNullOrEmpty(cssClass))
                field.CssClass = cssClass;

            return field;
        }

        public void UpdateFieldValues(FieldContainer dataSrc)
        {
            foreach(var dst in Fields)
            {
                var srcField = dataSrc.Fields.Where(f => f.Id == dst.Id).FirstOrDefault();
                if (srcField != null)
                    dst.UpdateValues(srcField);
            }
        }
    }
}

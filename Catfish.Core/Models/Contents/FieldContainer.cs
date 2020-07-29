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
        public XmlModelList<BaseField> Fields { get; protected set; }

        public FieldContainer(string tagName) : base(tagName) { Initialize(eGuidOption.Ignore); }
        public FieldContainer(XElement data) : base(data) { Initialize(eGuidOption.Ignore); }
        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            Fields = new XmlModelList<BaseField>(GetElement(FieldContainerTag, true), true, BaseField.FieldTagName);
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

        ////public int SetFieldValue<T>(string fieldName, string fieldNameLang, string val, string valLanguage, int valueIndex = 0)
        ////    where T : BaseField
        ////{
        ////    //Select the field of which the name is given in the fieldNameLanguage
        ////    BaseField field = GetFieldByName<T>(fieldName, fieldNameLang);

        ////    //Set the value of the selected field in the language given by valLanguage
        ////    return field.SetValue(val, valLanguage, valueIndex);
        ////}

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

    }
}

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
        public XmlModelList<BaseField> Fields { get; set; }

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
        public BaseField GetFieldByName(string fieldName, string lang)
        {
            foreach (var field in Fields)
            {
                if (field.Name.Values.Where(v => v.Language == lang && v.Value == fieldName).FirstOrDefault() != null)
                    return field;
            }
            return null;
        }

        public int SetFieldValue(string filedName, string fieldNameLanguage, string val, string valLanguage, int valueIndex = 0)
        {
            //Select the field of which the name is given in the fieldNameLanguage
            BaseField field = GetFieldByName(filedName, fieldNameLanguage);

            //Set the value of the selected field in the language given by valLanguage
            return field.SetValue(val, valLanguage, valueIndex);
        }
    }
}

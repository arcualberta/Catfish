using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class TextField : BaseField
    {
        public static readonly string ValuesTag = "values";
        public static readonly string ValueTag = "value";
        public TextField() { }
        public TextField(XElement data) : base(data) { }
        public TextField(string name, string desc, string lang = null) : base(name, desc, lang) { }


        public XmlModelList<MultilingualText> Values { get; set; }
        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            XmlModel xml = new XmlModel(Data);
            Values = new XmlModelList<MultilingualText>(xml.GetElement(ValuesTag, true), true, ValueTag);
        }

        public int SetValue(string val, string lang, int valueIndex = 0)
        {
            if(Values.Count <= valueIndex)
            {
                Values.Add(new MultilingualText(ValueTag));
                valueIndex = Values.Count - 1;
            }
            Values[valueIndex].SetContent(val, lang);
            return valueIndex;
        }

        public string GetValue(string lang, int valueIndex = 0)
        {
            return (Values.Count <= valueIndex) ? null : Values[valueIndex].GetContent(lang);
        }

    }
}

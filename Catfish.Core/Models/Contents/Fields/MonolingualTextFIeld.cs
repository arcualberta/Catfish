using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class MonolingualTextField : BaseField
    {
        public XmlModelList<Text> Values { get; protected set; }

        public MonolingualTextField() : base() { }
        public MonolingualTextField(XElement data) : base(data) { }
        public MonolingualTextField(string name, string desc, string lang = null) : base(name, desc, lang) { }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            // Populating the value list.
            Values = new XmlModelList<Text>(Data, true, Text.TagName);
        }

        public int SetValue(object val, int valueIndex = 0)
        {
            if (Values.Count <= valueIndex)
            {
                Values.Add(new Text());
                valueIndex = Values.Count - 1;
            }
            Values[valueIndex].Value = val == null ? "" : val.ToString();
            return valueIndex;
        }

        public string GetValue(int valueIndex = 0)
        {
            return Values[valueIndex].Value;
        }
    }
}

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
        public TextField() { }
        public TextField(XElement data) : base(data) { }

        public XmlModelList<MultilingualText> Values { get; set; }
        public TextField(string name, string desc, string lang = null) : base(name, desc, lang) { }

        public static string ValuesTag = "values";
        public static string ValueTag = "value";
        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            XmlModel xml = new XmlModel(Data);
            Values = new XmlModelList<MultilingualText>(xml.GetElement(ValuesTag, true), true, ValueTag);

            ////if(Values.Count == 0)
            ////{
            ////    //TODO: get the list of languages from the configuration file.
            ////    List<string> languages = Name.Values.Select(v => v.Language).Distinct().ToList();

            ////    MultilingualText value = new MultilingualText("value");
            ////    Values.Add(value);
            ////    foreach(var lang in languages)
            ////        value.SetContent("", lang);
            ////}

        }
    }
}

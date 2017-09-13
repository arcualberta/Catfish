using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Catfish.Core.Models.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Catfish.Core.Models.Metadata
{
    [Ignore]
    public partial class OptionsField: MetadataField
    {
        [DataType(DataType.MultilineText)]
        [TypeLabel("List of options, one option per line")]
        public string Options
        {
            get
            {
                return GetOptions("en");
            }
            //set;
        }

        public string GetOptions(string lang = null)
        {
            string result = null;
            if (lang == null)
                lang = DefaultLanguage;

            XElement options_element = Data.Element("options");
            if (options_element != null)
            {
                IEnumerable<XElement> option_text_elements = GetChildTextElements("option", options_element, lang);
                IEnumerable<string> options = option_text_elements.Select(op => op.Value);
                result = string.Join("\n", options);
            }
            return result;
        }

        
        public override void UpdateValues(XmlModel src)
        {
            //TODO: Deal with multiple languages later
            //XXX Add selected options
            // Omar

            OptionsField optionsField = src as OptionsField;
            var test = optionsField.GetValues();
            System.Diagnostics.Debug.WriteLine(optionsField);
            System.Diagnostics.Debug.WriteLine(test);
            System.Diagnostics.Debug.WriteLine(test.Count());
            foreach (var item in test)
            {
                System.Diagnostics.Debug.WriteLine(item);
            }
            System.Diagnostics.Debug.WriteLine("XXXX");

            // change from text entries to selected attributes
            //this.SetValues(src.GetValues());
            this.SetMultipleValues(optionsField.GetValues());
        }
        
        ////////public override XElement ToXml()
        ////////{
        ////////    XElement ele = base.ToXml();
        ////////    XElement options = new XElement("Options");
        ////////    ele.Add(options);

        ////////    ////options.Value = Options;
        ////////    foreach (var x in Options.Split(new char[] { '\n' }))
        ////////    {
        ////////        string opStr = x.Trim();
        ////////        if (!string.IsNullOrEmpty(opStr))
        ////////        {
        ////////            XElement op = new XElement("Option") { Value = opStr };
        ////////            options.Add(op);
        ////////        }
        ////////    }
        ////////    return ele;
        ////////}

        ////////public override void Initialize(XElement ele)
        ////////{
        ////////    base.Initialize(ele);
        ////////    var optionEnvelop = ele.Element("Options");
        ////////    List<string> options = new List<string>();
        ////////    foreach(var op in optionEnvelop.Elements("Option"))
        ////////    {
        ////////        options.Add(op.Value);
        ////////    }
        ////////    this.Options = string.Join("\n", options);
        ////////}
    }

}

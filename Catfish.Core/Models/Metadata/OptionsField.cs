using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Catfish.Core.Models.Attributes;
using System.Collections.Generic;

namespace Catfish.Core.Models.Metadata
{
    [Ignore]
    public partial class OptionsField: MetadataField
    {
        [DataType(DataType.MultilineText)]
        [TypeLabel("List of options, one option per line")]
        public string Options { get; set; }

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

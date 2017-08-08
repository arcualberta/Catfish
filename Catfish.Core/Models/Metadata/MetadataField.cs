using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Catfish.Core.Models.Attributes;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Catfish.Core.Models.Metadata
{
    public class MetadataField : XmlModel
    {
        [Display(Name="Is Required")]
        public bool IsRequired { get; set; }

        [Display(Name="Tooltip Help")]
        [DataType(DataType.MultilineText)]
        public string Help { get; set; }

        [XmlIgnore]
        [HiddenInput(DisplayValue = false)]
        public int MetadataSetId { get; set; }

        public override XElement ToXml()
        {
            XElement ele = base.ToXml();

            if (IsRequired)
                ele.SetAttributeValue("IsRequired", IsRequired);

            if (!string.IsNullOrWhiteSpace(Help))
                ele.Add(new XElement("Help") { Value = Help });

            return ele;
        }

        public override void Initialize(XElement ele)
        {
            base.Initialize(ele);
            this.IsRequired = bool.Parse(GetAtt(ele, "IsRequired", "false"));
            this.Help = GetChildText(ele, "Help");
        }

    }
}

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
using System.ComponentModel.DataAnnotations.Schema;

namespace Catfish.Core.Models.Metadata
{
    public class MetadataField : XmlModel
    {
        [NotMapped]
        public string Name
        {
            get
            {
                return GetName();
            }
            set
            {
                SetName(value);
            }
        }

        public override string GetTagName() { return "field"; }

        [NotMapped]
        [Display(Name="Is Required")]
        public bool IsRequired
        {
            get
            {
                if (Data != null)
                {
                    var att = Data.Attribute("IsRequired");
                    return att != null ? att.Value == "true" : false;
                }
                return false;
            }

            set
            {
                Data.SetAttributeValue("IsRequired", value);
            }
        }

        [NotMapped]
        [Display(Name="Tooltip Help")]
        [DataType(DataType.MultilineText)]
        public string Help
        {
            get
            {
                return GetHelp();
            }

            set
            {
                SetHelp(value);
            }
        }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [NotMapped]
        public string Value
        {
            get
            {
                IEnumerable<string> values = GetValues();
                return values.Any() ? values.First() : "";
            }

            set
            {
                List<string> values = new List<string>(){ value };
                SetValues(values);
            }
        }

        [DataType(DataType.MultilineText)]
        public string Description { get { return GetDescription(); } }



        ////[XmlIgnore]
        ////[HiddenInput(DisplayValue = false)]
        ////public int MetadataSetId { get; set; }

        ////////public override XElement ToXml()
        ////////{
        ////////    XElement ele = base.ToXml();

        ////////    if (IsRequired)
        ////////        ele.SetAttributeValue("IsRequired", IsRequired);

        ////////    if (!string.IsNullOrWhiteSpace(Help))
        ////////        ele.Add(new XElement("Help") { Value = Help });

        ////////    return ele;
        ////////}

        ////////public override void Initialize(XElement ele)
        ////////{
        ////////    base.Initialize(ele);
        ////////    this.IsRequired = bool.Parse(GetAtt(ele, "IsRequired", "false"));
        ////////    this.Help = GetChildText(ele, "Help");
        ////////}

    }
}

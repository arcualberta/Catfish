using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Catfish.Core.Models.Attributes;
using System.Xml.Serialization;

namespace Catfish.Core.Models.Metadata
{
    [XmlInclude(typeof(CheckBoxSet))]
    [XmlInclude(typeof(DateField))]
    [XmlInclude(typeof(DropDownMenu))]
    [XmlInclude(typeof(OptionsField))]
    [XmlInclude(typeof(RadioButtonSet))]
    [XmlInclude(typeof(TextArea))]
    [XmlInclude(typeof(TextField))]
    public class MetadataField
    {
        ////[XmlIgnore]
        ////[HiddenInput(DisplayValue = false)]
        ////public int Id { get; set; }

        [Rank(1)]
        [Required]
        [TypeLabel("String")]
        public string Name { get; set; }

        [Rank(2)]
        [DataType(DataType.MultilineText)]
        [TypeLabel("String")]
        public string Description { get; set; }

        public bool IsRequired { get; set; }

        [DataType(DataType.MultilineText)]
        public string ToolTip { get; set; }

        [XmlIgnore]
        [HiddenInput(DisplayValue = false)]
        public int MetadataSetId { get; set; }

        ////[XmlIgnore]
        ////[Ignore]
        ////public MetadataSet MetadataSet { get; set; }
    }
}

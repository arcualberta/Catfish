using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Catfish.Core.Models.Attributes;

namespace Catfish.Core.Models.Metadata
{
    public class MetadataField
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool IsRequired { get; set; }

        [DataType(DataType.MultilineText)]
        public string ToolTip { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int MetadataSetId { get; set; }

        [Ignore]
        public MetadataSet MetadataSet { get; set; }
    }
}

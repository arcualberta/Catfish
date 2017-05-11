using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Catfish.Core.Models.Metadata
{
    public class MetadataField
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int MetadataSetId { get; set; }

        [IgnoreDataMember]
        public MetadataSet MetadataSet { get; set; }
    }
}

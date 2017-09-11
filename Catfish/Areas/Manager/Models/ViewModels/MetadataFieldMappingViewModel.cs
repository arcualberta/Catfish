using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class MetadataFieldMappingViewModel
    {
        public int MetadataSetId { get; set; }

        public string MetadataSetName { get; set; }

        public int FieldName { get; set; }
    }
}
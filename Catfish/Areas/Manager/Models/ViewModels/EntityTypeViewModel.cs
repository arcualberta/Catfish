using Catfish.Core.Models;
using Catfish.Core.Models.Metadata;
using Catfish.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models
{
    public class EntityTypeViewModel
    {
        public EntityType EntityType { get; set; }

        public virtual ICollection<MetadataSet> AvailableMetadataSets { get; set; }

        public MetadataFieldMappingViewModel NameMapping { get; set; }

        public MetadataFieldMappingViewModel DescriptionMapping { get; set; }

        public List<EntityViewModel> MetadataSetSummary
        {
            get
            {
                return AvailableMetadataSets.Select(ms => new EntityViewModel(ms)).ToList();
            }
                 
        }

    }
}
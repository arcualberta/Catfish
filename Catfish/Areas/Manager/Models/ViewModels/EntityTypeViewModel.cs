using Catfish.Core.Models;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Metadata;
using Catfish.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class EntityTypeViewModel : KoBaseViewModel
    {
        public string TypeLabel { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<MetadataSetListItem> AvailableMetadataSets { get; set; }
        public List<MetadataSetListItem> SelectedMetadataSets { get; set; }

        public MetadataFieldMapping NameMapping { get; set; }

        public MetadataFieldMapping DescriptionMapping { get; set; }

        public EntityTypeViewModel(EntityType src, IQueryable<MetadataSet> metadataSets)
        {
            TypeLabelAttribute att = Attribute.GetCustomAttribute(src.GetType(), typeof(TypeLabelAttribute)) as TypeLabelAttribute;
            TypeLabel = att == null ? src.GetType().ToString() : att.Name;

            AvailableMetadataSets = new List<MetadataSetListItem>();
            foreach (var ms in metadataSets)
                AvailableMetadataSets.Add(new MetadataSetListItem(ms.Id, ms.Name));
        }
    }

    public class MetadataSetListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public MetadataSetListItem()
        {

        }

        public MetadataSetListItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class MetadataFieldMapping
    {
        public int MetadataSetId { get; set; }

        public string MetadataSetName { get; set; }

        public int FieldName { get; set; }
    }

}
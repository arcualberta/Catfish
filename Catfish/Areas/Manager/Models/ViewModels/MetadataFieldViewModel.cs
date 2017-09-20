using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class MetadataFieldViewModel : KoBaseViewModel
    {
        public string TypeLabel { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRequired { get; set; }
        public bool IsOptionField { get; set; }
        public string Options { get; set; }

        public MetadataFieldViewModel() { }

        public MetadataFieldViewModel(MetadataField src)
        {
            Name = src.Name;
            Description = src.Description;
            IsRequired = src.IsRequired;
            IsOptionField = typeof(OptionsField).IsAssignableFrom(src.GetType());

            TypeLabelAttribute att = Attribute.GetCustomAttribute(src.GetType(), typeof(TypeLabelAttribute)) as TypeLabelAttribute;
            TypeLabel = att == null ? src.GetType().ToString() : att.Name;

        }
    }
}
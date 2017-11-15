using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "FormExtension")]
    [ExportMetadata("Name", "Form")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class Form: CatfishRegion
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [Display(Name = "Entity Type Id")]
        public int EntityTypeId { get; set; }
        public string Pages { get; set; }

        [Display(Name = "Collection Id")]
        public int CollectionId { get; set; }
    }
}
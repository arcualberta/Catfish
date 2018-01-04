using Catfish.Core.Models;
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
    [ExportMetadata("InternalId", "FormContainerExtension")]
    [ExportMetadata("Name", "Form Container")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class FormContainer: CatfishRegion
    {
        [Display(Name = "Form Id")]
        public int FormId { get; set; }

        public Form Form { get; set; }

        [Display(Name = "Entity Type Id")]
        public int EntityTypeId { get; set; }

        [Display(Name = "Collection Id")]
        public int CollectionId { get; set; }
    }
}
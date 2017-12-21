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
    [ExportMetadata("InternalId", "CarouselExtension")]
    [ExportMetadata("Name", "Carousel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class BulletinBoard: CatfishRegion
    {
        [Display(Name ="Collection Id")]
        public int CollectionId { get; set; }

        [Display(Name ="Item Count")]
        public int ItemCount { get; set; }

        [Display(Name = "Select Randomly")]
        public bool SelectAtRandom { get; set; }

        [Display(Name = "Position at Random")]
        public bool PositionAtRandom { get; set; }
      
        [Display(Name = "Refresh Interval")]
        public int RefreshInterval { get; set; }
    }
}
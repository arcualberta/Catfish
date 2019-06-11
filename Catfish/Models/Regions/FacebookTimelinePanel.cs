using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "FacebookTimelinePanel")]
    [ExportMetadata("Name", "FacebookTimelinePanel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class FacebookTimelinePanel : CatfishRegion
    {
        [Display(Name = "Facebook Page Url")]
        public string TimelineUrl { get; set; }

        [Display(Name = "Width")]
        public int Width { get; set; }


        [Display(Name = "Height")]
        public int Height { get; set; }

        
    }
}
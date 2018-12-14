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
    [ExportMetadata("InternalId", "TwitterTimelinePanel")]
    [ExportMetadata("Name", "TwitterTimelinePanel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class TwitterTimelinePanel : CatfishRegion
    {
        [Display(Name = "Timeline Url")]
        public string TimelineUrl { get; set; }

        [Display(Name = "Number of Post")]
        public int NumOfPosts { get; set; }

        [Display(Name = "Theme")]
        public eTheme Theme { get; set; }

        //public override void InitManager(object model)
        //{
        //    base.InitManager(model);
        //}

        //public override object GetContent(object model)
        //{

        //    return base.GetContent(model);
        //}

       
    }
    public enum eTheme { white, dark }
}
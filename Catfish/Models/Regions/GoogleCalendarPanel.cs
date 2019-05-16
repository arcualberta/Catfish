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
    [ExportMetadata("InternalId", "GoogleCalendarPanel")]
    [ExportMetadata("Name", "GoogleCalendarPanel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class GoogleCalendarPanel : CatfishRegion
    {

        [Display(Name = "Calendar credentials")]
        public string CalendarCredentials { get; set; } = "Credentials";

        [Display(Name = "Day range")]
        public int DayRange { get; set; } = 10;

        [Display(Name = "Show event name")]
        public bool ShowEventName { get; set; } = true;

        [Display(Name = "Show event description")]
        public bool ShowEventDescription { get; set; } = true;

        [Display(Name = "Show event location")]
        public bool ShowEventLocation { get; set; } = true;

        [Display(Name = "Show event start")]
        public bool ShowEventStart { get; set; } = true;

        [Display(Name = "Show event end")]
        public bool ShowEventEnd { get; set; } = true;


        //[Display(Name = "Timeline Url")]
        //public string TimelineUrl { get; set; }

        //[Display(Name = "Number of Post")]
        //public int NumOfPosts { get; set; }

        //[Display(Name = "Theme")]
        //public eTheme Theme { get; set; }

        //public override void InitManager(object model)
        //{
        //    base.InitManager(model);
        //}

        //public override object GetContent(object model)
        //{

        //    return base.GetContent(model);
        //}


    }
    //public enum eTheme { white, dark }
}
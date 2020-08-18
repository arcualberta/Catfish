using Catfish.Helper;
using Catfish.Models.Regions;
using Microsoft.AspNetCore.Mvc;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;

namespace Catfish.Models
{
    [PageType(Title = "Social Media Page", UseBlocks = true)]
    [PageTypeRoute(Title = "Default", Route = "/mediapage")]
    public class MediaPage : Page<MediaPage>
    {
      
        [Region(ListTitle = "Title")]
        public GoogleCalendarRegion CalendarRegion { get; set; }

        [Region(ListTitle = "Title")]
        public FacebookTimeline FacebookTimeline { get; set; }

        [Region(ListTitle = "Title")]
        public TwitterRegion TwitterRegion { get; set; }

        [Region(ListTitle = "Title")]
        public StyleRegion Styles { get; set; }
       
        public MediaPage()
        {
          
        }

       
    }
}

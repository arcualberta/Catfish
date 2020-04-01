using Catfish.Models.Regions;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;

namespace Catfish.Models
{
    [PageType(Title = "Two Cols Page", UseBlocks = true)]
    [PageTypeRoute(Title = "Default", Route = "/colspage")]
    public class ColsPage : Page<ColsPage>
    {
       
        [Region(ListTitle = "Title")]
        public GoogleCalendarRegion CalendarRegion { get; set; }

       public ColsPage()
        {

        }
       
    }
}

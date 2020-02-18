using Catfish.Models.Regions;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;

namespace Catfish.Models
{
    [PageType(Title = "Main page", UseBlocks=true)]
    [PageTypeRoute(Route = "/main")]
    public class MainPage : Page<MainPage>
    {

        /// <summary>
        /// Gets/sets the page ImapeMap.
        /// [Region(Display = RegionDisplayMode.Setting)] //to display in "settings"
        /// [Region(ListTitle = "Title")] //display on the main area as separate tab
        /// </summary>

        [Region(ListTitle = "Title")]
        public ImageMap Map { get; set; }
       // public ImageField Map { get; set; }

        public MainPage() { }

    }
}

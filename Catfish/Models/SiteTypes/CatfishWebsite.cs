using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;
using Piranha.Extend.Fields;
using Catfish.Models.Regions;

namespace Catfish.Models.SiteTypes
{
    [SiteType(Title = "Basic Site")]
    public class CatfishWebsite : SiteContent<CatfishWebsite>
    {
        [Region(Title = "Keywords", Display = RegionDisplayMode.Setting)]
        public TextField Keywords { get; set; }

        [Region(Title = "Categories", Display = RegionDisplayMode.Setting)]
        public TextField Categories { get; set; }
        [Region(Title = "Header", Display = RegionDisplayMode.Setting)]
        public Header HeaderContents { get; set; }
        [Region(Title = "Footer", Display = RegionDisplayMode.Setting)]
        public Footer FooterContents { get; set; }

        public CatfishWebsite()
        {
            Keywords = new TextField();
            HeaderContents = new Header();
            FooterContents = new Footer();
        }

    }
}

using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;
using Piranha.Extend.Fields;

namespace CatfishWebExtensions.Models.Sites
{
    [SiteType(Title = "Basic Site")]
    public class CatfishWebsite : SiteContent<CatfishWebsite>
    {
        [Region(Title = "Keywords", Display = RegionDisplayMode.Setting)]
        public TextField Keywords { get; set; }

        [Region(Title = "Categories", Display = RegionDisplayMode.Setting)]
        public TextField Categories { get; set; }



        public CatfishWebsite()
        {
            Keywords = new TextField();
            Categories = new TextField();
        }

    }
}

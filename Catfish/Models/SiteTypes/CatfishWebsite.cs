using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;
using Piranha.Extend.Fields;

namespace Catfish.Models.SiteTypes
{
    [SiteType(Title = "Basic Site")]
    public class CatfishWebsite : SiteContent<CatfishWebsite>
    {
        [Region(Title = "Keywords", Display = RegionDisplayMode.Setting)]
        public TextField Keywords { get; set; }

        [Region(Title = "Categories", Display = RegionDisplayMode.Setting)]
        public TextField Categories { get; set; }

        [Region(Title = "Footer", Display = RegionDisplayMode.Setting)]
        public Footer FooterContents { get; set; }

        public CatfishWebsite()
        {
            Keywords = new TextField();
            FooterContents = new Footer();
        }

    }

    public class Footer
    {
        [Field(Title = "Footer logo")]
        public ImageField Logo { get; set; }

        [Field(Title = "Footer text")]
        public HtmlField Text { get; set; }

        [Field(Title = "Javascript")]
        public TextField Javascript { get; set; }

        [Field(Title = "Css")]
        public TextField Css { get; set; }
    }
}

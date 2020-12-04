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

    public class Header
    {
        //TODO this one doesn't do anything yet
        [Field(Title = "Display navigation menu", Placeholder = "If the site will display sub menu")]
        public CheckBoxField EnabledSubMenu { get; set; } = true;
        [Field(Title = "General Header Settings", Placeholder = "Remove the Website Title")]
        public CheckBoxField RemoveWebsiteTitle { get; set; }
        [Field(Title = "General Header Settings", Placeholder = "Remove Default Page Titles")]
        public CheckBoxField RemovePageTitles { get; set; }
        [Field(Title = "Header Background Color", Placeholder = "Please enter a hex value, including the hashtag, or color name")]
        public StringField BackgroundColor { get; set; }
        //[Field(Title = "Header Text Color", Placeholder = "Please enter a hex value, including the hashtag, or color name")]
        //public StringField TextColor { get; set; }

        [Field(Title = "Header Contents")]
        public HtmlField Text { get; set; }

        [Field(Title = "Javascript")]
        public TextField Javascript { get; set; }

        [Field(Title = "Css")]
        public TextField Css { get; set; }
    }

    public class Footer
    {
        [Field(Title = "Footer logo")]
        public ImageField Logo { get; set; }

        [Field(Title = "Footer Contents")]
        public HtmlField Text { get; set; }

        [Field(Title = "Javascript")]
        public TextField Javascript { get; set; }

        [Field(Title = "Css")]
        public TextField Css { get; set; }

        [Field(Title = "Enabled SubMenu", Placeholder = "If the site will display sub menu")]
        public CheckBoxField EnabledSubMenu { get; set; } = true;
    }
}

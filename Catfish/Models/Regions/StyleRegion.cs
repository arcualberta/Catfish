using Piranha.Extend;
using Piranha.Extend.Fields;


namespace Catfish.Models.Regions
{
    public class StyleRegion
    {
        [Field(Title="Is Two Columns Layout?")]
       public CheckBoxField IsTwoColumnsLayout { get; set; }
        [Field(Title = "Css class name for main contain")]
        public StringField MainContainCssClass { get; set; }
        [Field(Title = "Css class name for social media contain")]
        public StringField SocialMediaContainCssClass { get; set; }

        public StyleRegion() { }
    }
}

using Catfish.Models.Fields;
using Piranha.Extend;
using Piranha.Extend.Fields;

namespace Catfish.Models.Regions
{
    public class TwitterRegion
    {
        [Field(Title = "Title", Placeholder = "Tweet by @ARC")]
        public StringField Title { get; set; }
        [Field(Title ="TwitterRegion Url", Placeholder = "https://twitter.com/ualberta")]
        public StringField TwitterUrl { get; set; }
        [Field]
        public NumberField Width { get; set; }
        [Field]
        public NumberField Height { get; set; }

        public TwitterRegion() { }
    }
}

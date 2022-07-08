using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;

namespace Catfish.Models.Regions
{
    public class Hero
    {
        /// <summary>
        /// Gets/sets the optional subtitle.
        /// </summary>
        [Field(Options = FieldOption.HalfWidth)]
        public StringField Subtitle { get; set; }

        /// <summary>
        /// Gets/sets the optional primary image.
        /// </summary>
        [Field(Title = "Primary Image", Options = FieldOption.HalfWidth)]
        public ImageField PrimaryImage { get; set; }

        /// <summary>
        /// Gets/sets the optional ingress.
        /// </summary>
        [Field]
        [FieldDescription("Optional text that is shown on top of the background image")]
        public HtmlField Ingress { get; set; }
    }
}
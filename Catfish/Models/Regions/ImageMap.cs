using Catfish.Models.Fields;
using Piranha.Extend;
using Piranha.Extend.Fields;

namespace Catfish.Models.Regions
{
    public class ImageMap 
    {
        /// <summary>
        /// Gets/sets the optional primary image.
        /// </summary>
        [Field(Title = "Upload Image")]
        public ImageField PrimaryImage { get; set; }

        [Field]
        public TextField Description { get; set; }

        [Field(Title ="Extra Description")]
        public TextAreaField Description2 { get; set; }
    }
}

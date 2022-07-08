using Piranha;
using Piranha.Extend;
using Piranha.Extend.Blocks;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    // Category = "Media", Icon = "fas fa-image",
    [BlockType(Name = "Extended Image",  Component = "extended-image-block", IsUnlisted = true)]
    public class ExtendedImageBlock : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<ExtendedImageBlock>();

        /// <summary>
        /// Gets/sets the image body.
        /// </summary>
        public ImageField Body { get; set; }

        /// <summary>
        /// Gets/sets the selected image aspect.
        /// </summary>
        public SelectField<ImageAspect> Aspect { get; set; } = new SelectField<ImageAspect>();
        public TextField Title { get; set; }
        public TextField Description { get; set; }
        public TextField LinkText { get; set; }
        public TextField LinkUrl { get; set; }
        public CheckBoxField ImageComesFirst { get; set; }

        public override string GetTitle()
        {
            
            if (Body != null && Body.Media != null)
            {
                return Body.Media.Filename;
            }
            return "No image selected";
        }
        public string GetImageTitle()
        {

            return Title == null ? "" : Title.Value;
        }
        public string GetDescription()
        {
            return Description == null ? "" : Description.Value;
        }
        public string GetLinkText()
        {
            return LinkText == null ? "" : LinkText.Value;
        }
        public string GetLinkUrl()
        {
            return LinkUrl == null ? "#" : LinkUrl.Value;
        }
    }
}

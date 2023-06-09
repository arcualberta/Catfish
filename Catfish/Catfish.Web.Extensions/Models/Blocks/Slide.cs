using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CatfishWebExtensions.Constants.Enums;

namespace CatfishWebExtensions.Models.Blocks
{
    [BlockType(Name = "Slide", Category = "Content", Icon = "fas fa-window-maximize")]
    public class Slide : Block
    {
        public ImageField Image { get; set; }
        public StringField Title { get; set; }
        public StringField Link { get; set; }
        public HtmlField Content { get; set; }
        public SelectField<eUsage> Usage { get; set; }
        public SelectField<eSlideLayout> Layout { get; set; }
        [Display(Name = "Image Layout")]
        public SelectField<eImageLayout> ImageLayout { get; set; }
    }
}

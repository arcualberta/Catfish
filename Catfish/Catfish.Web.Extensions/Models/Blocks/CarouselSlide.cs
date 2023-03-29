using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CatfishWebExtensions.Constants.Enums;

namespace CatfishWebExtensions.Models.Blocks
{
    [BlockType(Name = "Carousel Slide", Category = "Content", Icon = "fas fa-window-maximize")]
    public class CarouselSlide : Block
    {
        public ImageField Image { get; set; }
        public StringField Title { get; set; }
        public HtmlField Content { get; set; }
        public SelectField<eCarouselLayout> Layout { get; set; }

    }
}

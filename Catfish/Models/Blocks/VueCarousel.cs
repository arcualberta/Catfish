using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Carousel", Category = "Media", Component = "vue-carousel", Icon = "fas fa-images")]
    public class VueCarousel : VueComponent
    {
        public List<CarouselSlide> Slides { get; set; }
        public override object GetData()
        {
            return Slides;
        }
    }

    public class CarouselSlide
    {

    }
}

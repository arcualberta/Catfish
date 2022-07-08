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
 //   [BlockGroupType(Name = "ExtendedGallery", Component = Category = "Media", Icon = "fas fa-images")]
    [BlockGroupType(Name = "Carousel", Category = "Media", Component = "vue-carousel", Icon = "fas fa-images")]
    [BlockItemType(Type = typeof(ExtendedImageBlock))]
    public class VueCarousel : VueComponentGroup, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<VueCarousel>();

        public StringField Width { get; set; }
        public StringField Height { get; set; }
    }
}

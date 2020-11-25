﻿using Piranha.Extend;
using Piranha.Extend.Blocks;
using Piranha.Extend.Fields;
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
        public SelectField<ImageAspect> Aspect { get; set; } = new SelectField<ImageAspect>(); 
        public TextField Title { get; set; } //should these be that or strings?
        public TextField Description { get; set; }
        public TextField LinkUrl { get; set; }
        public TextField LinkText { get; set; }

    }
}

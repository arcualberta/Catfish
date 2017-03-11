using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.Composition;
using Piranha.Extend;
using System.ComponentModel.DataAnnotations;


namespace Catfish.Models.Widgets
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "CarouselExtension")]
    [ExportMetadata("Name", "Carousel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class Carousel : Extension
    {
        [Display(Name = "Slide Count")]
        public int SlideCount { get; set; }

        [Display(Name = "Interval (ms)")]
        public int Interval { get; set; }

        [Display(Name = "Show Controls")]
        public bool EnableControls { get; set; }

        [Display(Name = "CSS Id")]
        public string CssId { get; set; }

        [Display(Name = "CSS Classes")]
        public string CssClasses { get; set; }

        [Display(Name = "CSS Styles")]
        public string CssStyles { get; set; }

        [Display(Name = "Slides")]
        public List<CarouselSlide> Slides { get; set; }

        public Carousel()
        {
            CssClasses = "col-lg-12";
            CssStyles = "height:400px;";
            Slides = new List<CarouselSlide>();
        }
    }

    public class CarouselSlide
    {
        [Display(Name = "Image")]
        public Piranha.Extend.Regions.ImageRegion Image { get; set; }

        [Display(Name = "Caption")]
        public string CaptionHeading { get; set; }

        [Display(Name = "Description")]
        public string CaptionDescription { get; set; }

        [Display(Name = "Link Text")]
        public string ReadMoreLinkText { get; set; }

        [Display(Name = "Link")]
        public string ReadMoreLink { get; set; }
    }
}
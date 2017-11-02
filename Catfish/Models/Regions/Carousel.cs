using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Piranha.Extend;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "CarouselExtension")]
    [ExportMetadata("Name", "Carousel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class Carousel : Extension
    {
        [Display(Name = "Image URLs")]
        public string ImageUrls { get; set; }

        [Display(Name = "Interval (ms)")]
        public int Interval { get; set; }

        [Display(Name = "Show Controls")]
        public bool EnableControls { get; set; }

        [Display(Name = "CSS Id")]
        public string CssId { get; set; }

        [Display(Name = "CSS Classes")]
        public string CssClasses { get; set; }

        [Display(Name = "Styles")]
        [DataType(DataType.MultilineText)]
        public string CssStyles { get; set; }

        public Carousel()
        {
            CssClasses = "col-lg-12";
            CssStyles = "height:400px;";
        }
    }
}
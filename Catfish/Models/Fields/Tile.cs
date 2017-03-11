using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Models.Fields
{
    public class Tile<T>
    {
        [Display(Name = "Caption")]
        public string CaptionHeading { get; set; }

        [Display(Name = "Description")]
        public T CaptionDescription { get; set; }

        [Display(Name = "Link Text")]
        public string ReadMoreLinkText { get; set; }

        [Display(Name = "Link")]
        public string ReadMoreLink { get; set; }
    }
}
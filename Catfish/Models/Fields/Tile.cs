using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Models.Fields
{
    public class Tile<MediaType, ContentType>
    {
        [Display(Name = "Image")]
        public MediaType PrimaryMedia { get; set; }
        
        [Display(Name = "Caption")]
        public string CaptionHeading { get; set; }

        [Display(Name = "Description")]
        public ContentType CaptionDescription { get; set; }

        [Display(Name = "Link Text")]
        public string ReadMoreLinkText { get; set; }

        [Display(Name = "Link")]
        public string ReadMoreLink { get; set; }
    }
}
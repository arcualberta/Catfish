using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Models.Fields
{
    public class Tile<MediaType, BodyType>
    {
        [Display(Name = "Image")]
        public MediaType PrimaryMedia { get; set; }
        
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Body")]
        public BodyType Body { get; set; }

        [Display(Name = "Link Text")]
        public string ReadMoreLinkText { get; set; }

        [Display(Name = "Link")]
        public string ReadMoreLink { get; set; }
    }
}
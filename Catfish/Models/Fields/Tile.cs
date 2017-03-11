using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Models.Fields
{
    [Serializable]
    public class Tile<MediaType>
    {
        [Display(Name = "Image")]
        public MediaType PrimaryMedia { get; set; }
        
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Body")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Display(Name = "Link Text")]
        public string ReadMoreLinkText { get; set; }

        [Display(Name = "Link")]
        public string ReadMoreLink { get; set; }
    }
}
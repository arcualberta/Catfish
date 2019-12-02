using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "VimeoPanel")]
    [ExportMetadata("Name", "VimeoPanel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class VimeoPanel : CatfishRegion
    {
       

        [Display(Name = "Vimeo Embed Code")]
        public string VimeoEmbedCode { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
       //start date when the video is available for user
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; } //end date when the video will be taking down
        [Display(Name = "Pre Date Message")]
        public string BeforeMessage { get; set; } // this message will be display when the start date is not starting yet
        [Display(Name = "Post Date Message")]
        public string AfterMessage { get; set; } // this message will be display when the end date has been reached


        //public override void InitManager(object model)
        //{
        //    base.InitManager(model);
        //}

        //public override object GetContent(object model)
        //{

        //    return base.GetContent(model);
        //}


    }

}
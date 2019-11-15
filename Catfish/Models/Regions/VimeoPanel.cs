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
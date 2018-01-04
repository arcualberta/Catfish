using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Models.Regions.Html
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "HtmlTag")]
    [ExportMetadata("Name", "HTML Tag")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class HtmlTag : CatfishRegion
    {
        [Display(Name = "Tag Name")]
        public string TagName { get; set; }

        [Display(Name = "Type")]
        public bool IsOpen { get; set; }

        public HtmlTag()
        {
            IsOpen = true;
        }
    }
}
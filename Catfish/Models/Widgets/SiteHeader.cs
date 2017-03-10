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
    [ExportMetadata("InternalId", "SiteHeader")]
    [ExportMetadata("Name", "SiteHeader")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class SiteHeader : Extension
    {
        [Display(Name = "Logo URL")]
        public string Logo { get; set; }

        [Display(Name = "Tagline")]
        public string Tagline { get; set; }

    }
}
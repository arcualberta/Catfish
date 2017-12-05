using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Catfish.Models.Regions.Html
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "Css")]
    [ExportMetadata("Name", "Css Section")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class Css : Piranha.Extend.Extension
    {
        public string Content { get; set; }
    }
}
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Catfish.Models.Regions.Html
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "Javascript")]
    [ExportMetadata("Name", "Javascript Section")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class Javascript: CatfishRegion
    {
        public string Content { get; set; }
    }
}
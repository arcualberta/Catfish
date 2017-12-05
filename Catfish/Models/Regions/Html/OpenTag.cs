using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Catfish.Models.Regions.Html
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "OpenTag")]
    [ExportMetadata("Name", "Open Tag")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class OpenTag : HtmlTag
    {
    }
}
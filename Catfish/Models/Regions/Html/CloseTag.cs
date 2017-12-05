using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Catfish.Models.Regions.Html
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "CloseTag")]
    [ExportMetadata("Name", "Close Tag")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class CloseTag : HtmlTag
    {
    }
}
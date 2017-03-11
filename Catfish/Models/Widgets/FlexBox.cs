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
    [ExportMetadata("InternalId", "FlexBoxExtension")]
    [ExportMetadata("Name", "FlexBox")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class FlexBox : Extension
    {
        [Display(Name = "Item Count")]
        public int ItemCount { get; set; }

        public List<FlexBoxItem> Items { get; set; }
    }

    public class FlexBoxItem
    {
        public string Title { get; set; }

        public string Content { get; set; }
    }
}
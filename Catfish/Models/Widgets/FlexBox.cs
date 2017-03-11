using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.Composition;
using Piranha.Extend;
using System.ComponentModel.DataAnnotations;

using Catfish.Models.Fields;

namespace Catfish.Models.Widgets
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "FlexBoxExtension")]
    [ExportMetadata("Name", "FlexBox")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class FlexBox : Extension
    {
        [Display(Name = "Panel Classes")]
        public string PanelClasses { get; set; }

        [Display(Name = "Item Classes")]
        public string ItemClasses { get; set; }

        [Display(Name = "Styles")]
        public string Styles { get; set; }

        public List<ImageTile> Items { get; set; }

        public FlexBox()
        {
            Items = new List<ImageTile>();
        }
    }
}
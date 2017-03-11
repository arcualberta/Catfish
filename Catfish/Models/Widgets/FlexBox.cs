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
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Item Count")]
        public int ItemCount { get; set; }

        [Display(Name = "CSS Id")]
        public string CssId { get; set; }

        [Display(Name = "Panel Classes")]
        public string PanelClasses { get; set; }

        [Display(Name = "Item Classes")]
        public string ItemClasses { get; set; }

        [Display(Name = "Styles")]
        [DataType(DataType.MultilineText)]
        public string Styles { get; set; }

        [Display(Name = "Clear Row")]
        public bool ClearRow { get; set; }

        public List<ImageTile> Items { get; set; }

        public FlexBox()
        {
            PanelClasses = "col-lg-12";
            ItemClasses = "col-md-4";
            Items = new List<ImageTile>();
        }
    }
}
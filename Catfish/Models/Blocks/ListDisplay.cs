using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Piranha.Extend.Fields;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Catfish.Models.Blocks
{
    public enum ColumnOptions : int
    {
        [Display(Description = "50-50 Column")]
        EvenColumns = 0,
        [Display(Description = "1/3 Item Column, 2/3 Display Column")]
        MoreDisplayThanItem = 1,
        [Display(Description = "2/3 Item Column, 1/3 Display Calendar")]
        MoreItemThanDisplay = 2,
        [Display(Description = "1/4 Item Column, 3/4 Display Calendar")]
        MoreDisplayThanItem2 = 3,
        [Display(Description = "3/4 Item Column, 1/4 Display Calendar")]
        MoreItemThanDisplay2 = 4,
    }

    public enum ItemPanelDirection : int
    {
        [Display(Description = "Left")]
        Left = 0,
        [Display(Description = "Right")]
        Right = 1,
        [Display(Description = "Top of Screen")]
        TopOfScreen = 2,
        [Display(Description = "Bottom of Screen")]
        BottomOfScreen = 3
    }

    [BlockGroupType(Name = "ListDisplay", Category = "Content", Component = "vue-list-display", Icon = "fas fa-images")]
    [BlockItemType(Type = typeof(SingleListItem))]
    public class ListDisplay : VueComponentGroup
    {
        public SelectField<ItemPanelDirection> ItemList { get; set; }
        public SelectField<ColumnOptions> ColumnWidth { get; set; }
    }
}

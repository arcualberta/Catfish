using Piranha.Extend;
using Piranha.Extend.Blocks;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
 This single list item is paired with the ListDisplay blockgroup.
 */

namespace Catfish.Models.Blocks
{
    public class SingleListItem : Block
    {
        public ImageField ItemImage { get; set; }
        public TextField ItemTitle { get; set; }
        public TextField ItemSubtitle { get; set; }
        //can't put blocks within blocks for block group so this will have to do
        public HtmlBlock ItemContents { get; set; } 
    }
}

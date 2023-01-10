using Piranha;
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
    [BlockType(Name = "Dont Choose Me", Component = "vue-single-list-item", IsUnlisted = true)]
    public class SingleListItem : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<SingleListItem>();

        public ImageField Body { get; set; }
        public ImageField ItemImage { get; set; }
        public StringField ItemTitle { get; set; }
        public StringField ItemSubtitle { get; set; }
        //can't put blocks within blocks for block group so this will have to do
        public HtmlField ItemContents { get; set; }
    }
}

using Catfish.Models.Blocks;
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;

namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Item Details", Category = "Submissions", Component = "item-details", Icon = "fas fa-light fa-memo")]
    public class ItemDetails : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<ItemDetails>();

        public TextField QueryParameter { get; set; }

    }
}

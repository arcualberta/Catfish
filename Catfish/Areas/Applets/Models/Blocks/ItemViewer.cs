using Catfish.Core.Models;
using Catfish.Models.Blocks;
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;

namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Item Viewer", Category = "Content", Component = "item-viewer", Icon = "fas fa-edit")]
    public class ItemViewer : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<ItemViewer>();
    }
}

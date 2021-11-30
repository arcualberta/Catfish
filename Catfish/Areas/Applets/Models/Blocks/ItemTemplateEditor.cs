using Catfish.Core.Models;
using Catfish.Models.Blocks;
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;

namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Item Template Editor", Category = "Content", Component = "item-template-editor", Icon = "fas fa-edit")]
    public class ItemTemplateEditor : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<ItemTemplateEditor>();

        public TextField Label { get; set; } = "Item Template Editor";
    }
}

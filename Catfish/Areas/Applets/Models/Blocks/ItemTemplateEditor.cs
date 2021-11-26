using Catfish.Core.Models;
using Piranha.Extend;
using Piranha.Extend.Fields;

namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Item Template Editor", Category = "Content", Component = "item-template-editor", Icon = "fas fa-edit")]
    public class ItemTemplateEditor : Block
    {
        public TextField Label { get; set; } = "Item Template Editor";
    }
}

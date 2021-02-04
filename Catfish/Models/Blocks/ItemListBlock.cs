using Catfish.Core.Models;
using Catfish.Models.Fields;
using Piranha.Extend;
using Piranha.Extend.Fields;


namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Item List", Category = "Workflow", Component = "item-list", Icon = "far fa-list-alt")]
    public class ItemListBlock : Block
    {
        public TextField EntityTemplateId { get; set; }
        public TextField CollectionId { get; set; }
        public CatfishSelectList<Collection> Collections { get; set; }
        public CatfishSelectList<ItemTemplate> ItemTemplates { get; set; }

        public TextField SelectedCollection { get; set; }

        public TextField SelectedItemTemplate { get; set; }

        public TextField AuthorizationFailureMessage { get; set; }
    }
}
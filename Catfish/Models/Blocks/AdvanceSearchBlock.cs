using Catfish.Core.Models;
using Catfish.Models.Fields;
using Piranha.Extend;
using Piranha.Extend.Fields;


namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Advance Search", Category = "Control", Component = "advance-search", Icon = "fas fa-search")]

    public class AdvanceSearchBlock : Block
    {
        public CatfishSelectList<Collection> Metadatasets { get; set; }
        public CatfishSelectList<ItemTemplate> ItemTemplates { get; set; }

        public TextField SelectedMetadataset { get; set; }

        public TextField SelectedItemTemplate { get; set; }

        public TextField SelectedRenderingType { get; set; }

    }
}

using Catfish.Core.Models;
using Catfish.Models.Blocks;
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;

namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Submission View", Category = "Submissions", Component = "item-viewer", Icon = "fas fa-eye")]
    public class ItemViewer : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<ItemViewer>();

        public TextField QueryParameter { get; set; }

    }
}

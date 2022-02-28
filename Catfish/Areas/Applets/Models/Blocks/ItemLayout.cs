using Catfish.Models.Blocks;
using Piranha.Extend;

using Piranha;

using Piranha.Extend.Fields;

using Catfish.Models.Fields;
using Catfish.Core.Models;

namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Item Layout", Category = "Submissions", Component = "item-layout", Icon = "fas fa-chart-pie")]
    public class ItemLayout : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<Report>();

        public TextField SelectedGroupId { get; set; }
        public CatfishSelectList<ItemTemplate> ItemTemplates { get; set; }
        public TextField SelectedItemTemplateId { get; set; }
       
        public TextField SelectedFieldList { get; set; } 
        public TextField SelectedField { get; set; }
       
        public TextField SelectedFormId { get; set; }

      
    }

   
}

using Catfish.Models.Blocks;
using Piranha.Extend;

using Piranha;

using Piranha.Extend.Fields;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Catfish.Models.Fields;
using Catfish.Core.Models;

namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Report", Category = "Submissions", Component = "report", Icon = "fas fa-chart-pie")]
    public class Report : Block, ICatfishBlock
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

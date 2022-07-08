using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Vue List", Category = "Content", Component = "vue-list", Icon = "fas fa-list")]
    public class VueList : VueComponent, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<VueList>();

        public TextField Items { get; set; }
    }
}

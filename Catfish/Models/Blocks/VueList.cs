using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Vue List", Category = "Content", Component = "vue-list", Icon = "fas fa-list")]
    public class VueList : VueComponent
    {
        public TextField Items { get; set; }

        public override object GetData()
        {
            return Items.Value.Split(",", StringSplitOptions.RemoveEmptyEntries);
        }

    }
}

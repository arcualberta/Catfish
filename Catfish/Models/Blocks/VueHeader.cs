using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Vue Header", Category = "Content", Component = "vue-header", Icon = "fas fa-list")]
    public class VueHeader : VueComponent
    {
        public TextField Items { get; set; }
    }
}
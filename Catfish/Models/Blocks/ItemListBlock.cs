using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Item List", Category = "Workflow", Component = "item-list", Icon = "far fa-list-alt")]
    public class ItemListBlock : Block
    {
        public TextField EntityTemplateId { get; set; }
        public TextField CollectionId { get; set; }
    }
}
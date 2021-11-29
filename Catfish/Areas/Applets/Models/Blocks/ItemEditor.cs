using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Models.Fields;
using Piranha.Extend;
using Piranha.Extend.Fields;

namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Item Editor", Category = "Content", Component = "item-editor", Icon = "fas fa-edit")]
    public class ItemEditor:Block
    {
        public TextField ItemId { get; set; }
        [Display(Name = "Css Class")]
        public StringField CssClass { get; set; }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Blocks
{
    [BlockGroupType(Name = "Carousel", Category = "Content", Icon = "fas fa-caret-square-down")]
    public class Carousel : BlockGroup
    {
        public StringField CssClasses { get; set; }
    }
}

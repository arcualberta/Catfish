using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Css Block", Category = "Content", Component = "css-block", Icon = "fas fa-file-code")]
    public class CssBlock : Block
    { 
        public TextField CssVal { get; set; }

        public string GetCss()
        {
            if(CssVal != null)
            {
                return CssVal.Value;
            }

            return "";
        }
    }
}

using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    public class VueBlockTypeAttribute : BlockTypeAttribute
    {
        public bool UsesRazorTemplate { get; set; } = false;
    }
}

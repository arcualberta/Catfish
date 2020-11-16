using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    public abstract class VueComponent : Block
    {
        public abstract object GetData();
        public string Component
        {
            get
            {
                var blockTypeAttribute = Attribute.GetCustomAttribute(this.GetType(), typeof(BlockTypeAttribute)) as BlockTypeAttribute;
                if (blockTypeAttribute == null)
                    return null;
                else
                    return blockTypeAttribute.Component;
            }
        }
    }
}

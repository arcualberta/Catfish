using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models
{
    public class Item2 : Aggregation2
    {
        public override string GetTagName() { return "item"; }
    }
}

using Catfish.Models;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Helper
{
    public interface IBlockHelper
    {
        public IEnumerable<string> GetVueComponentNames(IList<Block> blocks);
    }
}

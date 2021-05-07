using Catfish.Models;
using Catfish.Models.Blocks;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Helper
{
    public class BlockHelper: IBlockHelper
    {
        public IEnumerable<string> GetVueComponentNames(IList<Block> blocks)
        {
            IEnumerable<string> names = blocks.Where(b => typeof(IVueComponent).IsAssignableFrom(b.GetType())) //Selecting all blocks of type IVueConponent or its derived type
                .Select(b => (b as IVueComponent).Component) //Selecting all those blocks as IVueConponent blocks and getting their names.
                .Distinct()
                .ToList();

            var blockGroups = blocks.Where(b => typeof(BlockGroup).IsAssignableFrom(b.GetType()))
                .Select(b => b as BlockGroup);

            foreach (var blockGroup in blockGroups)
            {
                var childNames = GetVueComponentNames(blockGroup.Items);
                names = names.Union(childNames);
            }

            return names;
        }
    }
}

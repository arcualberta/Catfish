using Catfish.Models.Blocks;
using Piranha;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Grid", Category = "Content", Component = "grid", Icon = "fas fa-th")]
	public class Grid: BlockGroup, ICatfishBlock
	{
        public void RegisterBlock() => App.Blocks.Register<Grid>();
	}
}

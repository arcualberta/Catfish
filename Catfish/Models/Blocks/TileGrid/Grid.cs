using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks.TileGrid
{
    public class Grid : Block
    {
        public ICollection<Tile> Tiles { get; set; } = new List<Tile>();
    }
}

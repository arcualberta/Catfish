using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks.TileGrid
{
    [BlockType(Name = "Tile Grid", Category = "Content", Component = "tile-grid", Icon = "far fa-square")]
    public class TileGrid : Block
    {
        public ICollection<Tile> Tiles { get; set; } = new List<Tile>();
    }
}

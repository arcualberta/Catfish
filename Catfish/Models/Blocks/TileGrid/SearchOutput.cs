using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks.TileGrid
{
    public class SearchOutput
    {
        public List<Tile> Items { get; set; } = new List<Tile>();
        public int First { get; set; }
        public int Last { get; set; }
        public int Count { get; set; }
    }
}

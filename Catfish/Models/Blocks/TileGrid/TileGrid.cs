using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks.TileGrid
{
    [BlockType(Name = "Tile Grid", Category = "Content", Component = "tile-grid", Icon = "fas fa-th")]
    public class TileGrid : Block
    {
        // public ICollection<Tile> Tiles { get; set; } = new List<Tile>();

        [Field(Title = "Keywords", Placeholder = "Please list keywords separated by a comma")]
        public TextField KeywordList { get; set; }

        public string GetKeywords()
        {
            if (KeywordList != null)
            {
                return KeywordList.Value;
            }

            return "";
        }
    }
}

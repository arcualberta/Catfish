using Catfish.Core.Models;
using Catfish.Models.Fields;
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
        public CatfishSelectList<Collection> Collections { get; set; }
        public TextField SelectedCollection { get; set; }

        public CatfishSelectList<ItemTemplate> ItemTemplates { get; set; }
        public TextField SelectedItemTemplate { get; set; }

        [Field(Title = "Css class for the Block")]
        public TextField BlockCss { get; set; }
        [Field(Title = "Css class for each Tile")]
        public TextField TileCss { get; set; }

        public TextField SelectedMapTitleId { get; set; }
        public TextField SelectedMapSubtitleId { get; set; }
        public TextField SelectedMapContentId { get; set; }
        public TextField SelectedMapThumbnailId { get; set; }
        public TextField DetailedViewUrl { get; set; }

        public TextField KeywordSourceId { get; set; }
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

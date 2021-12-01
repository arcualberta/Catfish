using Catfish.Core.Models;
using Catfish.Models.Blocks;
using Catfish.Models.Fields;
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Keyword Search", Category = "Control", Component = "keyword-search", Icon = "fas fa-search")]
    public class KeywordSearch : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<KeywordSearch>();

        [Field(Title = "Additional Keywords", Placeholder = "Please list keywords separated by comma")]
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

        public TextField ClassificationMetadataSetId { get; set; }
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

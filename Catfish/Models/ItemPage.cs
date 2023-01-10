using Catfish.Models.Fields;
using Catfish.Models.Regions;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models
{
    [PageType(Title = "Item page")]
    public class ItemPage : Page<ItemPage>
    {
        [Region(Title = "Keywords", Display = RegionDisplayMode.Setting)]
        public ControlledKeywordsField Keywords { get; set; } = new ControlledKeywordsField();

        [Region(Title = "Categories", Display = RegionDisplayMode.Setting)]
        public ControlledCategoriesField Categories { get; set; } = new ControlledCategoriesField();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        [Region(Title = "Page Settings", Display = RegionDisplayMode.Setting)]
        public PublishSettings PublishSettings { get; set; } = new PublishSettings();
    }
}

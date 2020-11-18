using Catfish.Models.Fields;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;
using System.Collections.Generic;

namespace Catfish.Models
{
    [PageType(Title = "Standard page")]
    public class StandardPage : Page<StandardPage>
    {
        [Region(Title = "Keywords", Display = RegionDisplayMode.Setting)]
        public ControlledKeywordsField Keywords { get; set; } = new ControlledKeywordsField();

        [Region(Title = "Categories", Display = RegionDisplayMode.Setting)]
        public ControlledCategoriesField Categories { get; set; } = new ControlledCategoriesField();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

    }
}
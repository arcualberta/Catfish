using System.Collections.Generic;
using Catfish.Models.Fields;
using Catfish.Models.Regions;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;


namespace Catfish.Models
{
    [PostType(Title = "Standard post")]
    public class StandardPost : Post<StandardPost>
    {
        /// <summary>
        /// Gets/sets the available comments if these
        /// have been loaded from the database.
        /// </summary>
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        [Region(Title = "Keywords", Display = RegionDisplayMode.Setting)]
        public ControlledKeywordsField Keywords { get; set; } = new ControlledKeywordsField();

        [Region(Title = "Categories", Display = RegionDisplayMode.Setting)]
        public ControlledCategoriesField Categories { get; set; } = new ControlledCategoriesField();

        [Region(Title = "Publish Settings", Display = RegionDisplayMode.Setting)]
        public PublishSettings PublishSettings { get; set; } = new PublishSettings();

    }
}

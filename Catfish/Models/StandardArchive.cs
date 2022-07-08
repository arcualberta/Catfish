using Catfish.Models.Fields;
using Catfish.Models.Regions;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;

namespace Catfish.Models
{
    [PageType(Title = "Standard archive", IsArchive = true)]
    public class StandardArchive : Page<StandardArchive>
    {
        /// <summary>
        /// The currently loaded post archive.
        /// </summary>
        public PostArchive<PostInfo> Archive { get; set; }

        [Region(Title = "Keywords", Display = RegionDisplayMode.Setting)]
        public ControlledKeywordsField Keywords { get; set; }

        [Region(Title = "Categories", Display = RegionDisplayMode.Setting)]
        public ControlledCategoriesField Categories { get; set; } = new ControlledCategoriesField();

        [Region(Title = "Publish Settings", Display = RegionDisplayMode.Setting)]
        public PublishSettings PublishSettings { get; set; } = new PublishSettings();

    }
}
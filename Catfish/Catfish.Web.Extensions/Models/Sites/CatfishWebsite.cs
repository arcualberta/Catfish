using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;
using Piranha.Extend.Fields;
using CatfishWebExtensions.Models.Regions;
using CatfishWebExtensions.Models.Sites.Headers;

namespace CatfishWebExtensions.Models.Sites
{
    [SiteType(Title = "Catfish Site")]
    public class CatfishWebsite : SiteContent<CatfishWebsite>
    {
        [Region(Title = "WebExtensions", Display = RegionDisplayMode.Setting)]
        public WebSettings WebSettings { get; set; }


        [Region(Title = "Workflow", Display = RegionDisplayMode.Setting)]
        public WorkflowSettings WorkflowSettings { get; set; }

        [Region(Title = "Header Settings", Display = RegionDisplayMode.Setting)]
        public DefaultHeader? DefaultHeader { get; set; } = null;
        public BiLevelHeader? BiLevelHeader { get; set; } = null;

        public CatfishWebsite()
        {
            WebSettings = new WebSettings();
            WorkflowSettings = new WorkflowSettings();
        }

    }
}

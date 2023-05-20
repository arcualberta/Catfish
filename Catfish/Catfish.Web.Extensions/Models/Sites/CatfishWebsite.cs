using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;
using Piranha.Extend.Fields;
using CatfishWebExtensions.Models.Regions;
using CatfishWebExtensions.Models.Sites.Headers;
using CatfishWebExtensions.Models.Sites.Footers;

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
        public DefaultHeader Header { get; set; } = null;

        [Region(Title = "Footer Settings", Display = RegionDisplayMode.Setting)]
        public DefaultFooter Footer { get; set; } = null;

        public CatfishWebsite()
        {
            WebSettings = new WebSettings();
            WorkflowSettings = new WorkflowSettings();
        }

    }
}

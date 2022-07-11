using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;
using Piranha.Extend.Fields;
using CatfishWebExtensions.Models.Regions;

namespace CatfishWebExtensions.Models.Sites
{
    [SiteType(Title = "Catfish Site")]
    public class CatfishWebsite : SiteContent<CatfishWebsite>
    {
        [Region(Title = "WebExtensions", Display = RegionDisplayMode.Setting)]
        public WebSettings WebSettings { get; set; }


        [Region(Title = "Workflow", Display = RegionDisplayMode.Setting)]
        public WorkflowSettings WorkflowSettings { get; set; }

        public CatfishWebsite()
        {
            WebSettings = new WebSettings();
            WorkflowSettings = new WorkflowSettings();
        }

    }
}

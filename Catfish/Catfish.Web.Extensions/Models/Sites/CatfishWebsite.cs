using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;
using Piranha.Extend.Fields;

namespace CatfishWebExtensions.Models.Sites
{
    [SiteType(Title = "Workflow Site")]
    public class WorkflowSite : SiteContent<WorkflowSite>
    {
        [Region(Title = "Test Field", Display = RegionDisplayMode.Setting)]
        public TextField TestField { get; set; }


        public WorkflowSite()
        {
            TestField = new TextField();
        }

    }
}

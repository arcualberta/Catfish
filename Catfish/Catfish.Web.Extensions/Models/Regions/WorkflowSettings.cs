using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Regions
{
    public class WorkflowSettings
    {
        [Field(Title = "Keywords")]
        public TextField Keywords { get; set; }

        [Field(Title = "Categories")]
        public TextField Categories { get; set; }

    }
}

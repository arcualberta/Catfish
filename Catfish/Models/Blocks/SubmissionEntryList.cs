using Catfish.Core.Models;
using Catfish.Services;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Submission Entry List", Category = "Workflow", Component = "submission-entry-list", Icon = "fas fa-th-list")]
    public class SubmissionEntryList : Block
    {
        public  TextField CssVal { get; set; }

        public string GetCss()
        {
            if (CssVal != null)
            {
                return CssVal.Value;
            }

            return "";
        }
    }
}

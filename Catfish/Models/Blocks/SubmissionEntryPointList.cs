using Catfish.Core.Models;
using Catfish.Services;
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Submission Entry Point List", Category = "Workflow", Component = "submission-entry-point-list", Icon = "fas fa-th-list")]
    public class SubmissionEntryPointList : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<SubmissionEntryPointList>();

        public  TextField CssVal { get; set; }

        public SubmissionEntryPointList()
        {
            CssVal = new TextField();
        }
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

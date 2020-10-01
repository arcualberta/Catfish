using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Submission Entry Point", Category = "Workflow", Component = "submission-entry-point", Icon = "fab fa-wpforms")]
    public class SubmissionEntryPoint : Block
    {
        public  TextField EntityTemplateId { get; set; }
        public TextField CollectionId { get; set; }
    }
}

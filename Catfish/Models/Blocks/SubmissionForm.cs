using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Submission Form", Category = "Workflow", Component = "submission-form", Icon = "fab fa-wpforms")]
    public class SubmissionForm : Block
    {
        public  TextField EntityTemplateId { get; set; }
        public TextField CollectionId { get; set; }
    }
}

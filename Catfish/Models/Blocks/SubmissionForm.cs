using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Submission Form", Category = "Workflow", Component = "submission-form", Icon = "fab fa-wpforms")]
    public class SubmissionForm : Block
    {
        [Display(Name = "Css Class")]
        public StringField CssClass { get; set; }

        [Display(Name = "Entity Template")]
        public TextField EntityTemplateId { get; set; }

        [Display(Name = "Collection")]
        public TextField CollectionId { get; set; }
    }
}

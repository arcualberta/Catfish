using Catfish.Core.Models;
using Catfish.Models.Fields;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Vue Form", Category = "Workflow", Component = "submission-form", Icon = "fab fa-wpforms")]
    public class VueSubmissionForm : Block, IVueComponent
    {
        [Display(Name = "Css Class")]
        public StringField CssClass { get; set; }

        [Display(Name = "Submission Confirmation")]
        public TextField SubmissionConfirmation { get; set; }

        public CatfishSelectList<Collection> Collections { get; set; }
        public CatfishSelectList<ItemTemplate> ItemTemplates { get; set; }

        public TextField SelectedCollection { get; set; }

        public TextField SelectedItemTemplate { get; set; }

        public TextField WorkflowFunction { get; set; }

        public TextField WorkflowGroup { get; set; }

        public CheckBoxField LinkToGroup { get; set; }

        public StringField GroupSelectorLabel { get; set; }
    }
}

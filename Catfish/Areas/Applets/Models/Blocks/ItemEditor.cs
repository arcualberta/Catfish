using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Models.Fields;
using Piranha.Extend;
using Piranha.Extend.Fields;

namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Item Editor", Category = "Content", Component = "item-editor", Icon = "fas fa-edit")]
    public class ItemEditor:Block
    {
        public Guid ItemId { get; set; }
        [Display(Name = "Css Class")]
        public StringField CssClass { get; set; }

        [Display(Name = "Submission Confirmation")]
        public TextField SubmissionConfirmation { get; set; }

        [Display(Name = "Authorization Failure Message")]
        public TextField AuthorizationFailureMessage { get; set; }

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

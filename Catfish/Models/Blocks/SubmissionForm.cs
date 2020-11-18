
using Catfish.Core.Models;
using Catfish.Models.Fields;

using Piranha.Extend;
using Piranha.Extend.Fields;
using System.ComponentModel.DataAnnotations;

namespace Catfish.Models.Blocks
{
  
    // private Enum eCollection = Enum.TryParse(EType, "Collection");
    [BlockType(Name = "Submission Form", Category = "Workflow", Component = "submission-form", Icon = "fab fa-wpforms")]
    public class SubmissionForm : Block
    {
        [Display(Name = "Css Class")]
        public StringField CssClass { get; set; }
 
        [Display(Name = "Submission Confirmation")]
        public TextField SubmissionConfirmation { get; set; }

         public CatfishSelectList<Collection> Collections { get; set; }
        public CatfishSelectList<ItemTemplate> ItemTemplates { get; set; }
      
        public TextField SelectedCollection { get; set; }
     
        public TextField SelectedItemTemplate { get; set; }
        public TextField Function { get; set; }
        public TextField Group { get; set; }
    }
}

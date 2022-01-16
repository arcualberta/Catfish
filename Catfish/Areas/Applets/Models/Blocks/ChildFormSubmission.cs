using Catfish.Core.Models;
using Catfish.Models.Blocks;
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;


namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Child Form Submission", Category = "Submissions", Component = "child-form-submission", Icon = "fas fa-share-square")]
    public class ChildFormSubmission : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<ChildFormSubmission>();
        public Catfish.Models.Fields.CatfishSelectList<ItemTemplate> ItemTemplates { get; set; }
        public TextField SelectedItemTemplate { get; set; }
        public TextField SelectedChildForm { get; set; }
        public TextField QueryParameter { get; set; }
        public CheckBoxField DisplayChildList { get; set; }
    }
}

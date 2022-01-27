using Catfish.Core.Models;
using Catfish.Models.Blocks;
using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;


namespace Catfish.Areas.Applets.Models.Blocks
{
    [BlockType(Name = "Form Submission", Category = "Submissions", Component = "form-submission", Icon = "fas fa-share-square")]
    public class FormSubmission : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<FormSubmission>();
        public Catfish.Models.Fields.CatfishSelectList<ItemTemplate> ItemTemplates { get; set; }
        public TextField SelectedItemTemplate { get; set; }
        public TextField SelectedForm { get; set; }
    }
}


namespace CatfishWebExtensions.Blocks
{
    [BlockGroupType(Name = "Accordion", Category = "Content", Icon = "fas fa-caret-square-down")]
    public class Accordion : BlockGroup
    {
        [Display(Name = "CSS Classes")]
        public StringField CSSClasses { get; set; }

    }
}

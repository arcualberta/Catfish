
namespace CatfishWebExtensions.Blocks
{
    [BlockGroupType(Name = "Accordion", Category = "Content", Icon = "fas fas-image")]
    public class Accordion : BlockGroup
    {
        [Display(Name = "CSS Classes")]
        public StringField CSSClasses { get; set; }

    }
}


namespace CatfishWebExtensions.Blocks
{
    [BlockType(Name = "Extended Image", Category = "Content", Icon = "fas fas-image")]
    public class ExtendedImage : Block
    {
        [Display(Name ="Image Source")]
        public ImageField Source { get; set; }

        [Display(Name ="Link URL")]
        public StringField LinkUrl { get; set; }

        [Display(Name = "Open on New Page")]
        public CheckBoxField OpenOnNewPage { get; set; }

        [Display(Name = "CSS Classes")]
        public StringField CSSClasses { get; set; }

    }
}

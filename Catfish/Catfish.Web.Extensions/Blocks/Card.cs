
namespace CatfishWebExtensions.Blocks
{
    [BlockType(Name = "Card", Category = "Content", Icon = "fas fas-image")]
    public class Card : Block
    {
        [Display(Name = "Title")]
        public StringField Title { get; set; }

        [Display(Name = "Content")]
        public HtmlField Content { get; set; }

    }
}

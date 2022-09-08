
namespace CatfishWebExtensions.Blocks
{
    [BlockType(Name = "Card", Category = "Content", Icon = "fas fa-window-maximize")]
    public class Card : Block
    {
        [Display(Name = "Title")]
        public StringField Title { get; set; }

        [Display(Name = "Content")]
        public HtmlField Content { get; set; }

    }
}

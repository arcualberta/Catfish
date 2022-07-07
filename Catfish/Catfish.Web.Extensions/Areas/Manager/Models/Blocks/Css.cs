
namespace Catfish.Web.Extensions.Areas.Manager.Blocks
{
    [BlockType(Name = "Css Block", Category = "Content", Component = "css", Icon = "fas fa-file-code")]
    public class Css : Block
    {
        public void RegisterBlock() => App.Blocks.Register<Css>();
        public TextField CssVal { get; set; } = "";

        public string GetCss()
        {
            if (CssVal != null)
            {
                return CssVal.Value;
            }

            return "";
        }
    }
}

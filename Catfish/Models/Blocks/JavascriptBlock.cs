using Piranha.Extend;
using Piranha.Extend.Fields;


namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Javascript Block", Category = "Content", Component = "javascript-block", Icon = "fas fa-js-square")]
    public class JavascriptBlock : Block
    {
        public TextField JavascriptCode {get;set;}

        public string GetJavascript()
        {
            if (JavascriptCode.Value != null)
            {

                return JavascriptCode.Value;
            }
            return "";
        }

    }
}

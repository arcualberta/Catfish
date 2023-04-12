using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Sites.Footers
{
    public class UpperLevel : LowerLevel
    {
        [Field(Title = "Upper Footer")]
        public HtmlField UpperFooter { get; set; }
    }
}

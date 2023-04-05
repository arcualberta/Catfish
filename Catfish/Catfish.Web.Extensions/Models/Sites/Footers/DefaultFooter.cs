using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Sites.Footers
{
    public class DefaultFooter
    {
        [Field(Title = "Footer Icon")]
        public ImageField Icon { get; set; }

        [Field(Title = "Footer Title")]
        public TextField FooterTitle { get; set; }

        [Field(Title = "Link")]
        public TextField Link { get; set; }
    }
}

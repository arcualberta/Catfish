using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Sites.Footers
{
    public class LowerLevel
    {
        [Field(Title = "Footer Logo")]
        public ImageField Logo { get; set; }

        [Field(Title = "Footer Title")]
        public TextField FooterTitle { get; set; }

        [Field(Title = "Copyright")]
        public TextField Copyright { get; set; }
    }
}

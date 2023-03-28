using CatfishWebExtensions.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Sites.Headers
{
    [HeaderModel(Name = "Default Header", ViewTemplate = "Header/_DefaultHeader")]
    public class DefaultHeader
    {
        [Field(Title = "Header Logo")]
        public ImageField Logo { get; set; }

        [Field(Title = "Header Title")]
        public TextField SiteTitle { get; set; }

        [Field(Title = "Css Classes")]
        public TextField CssClasses { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Piranha.Extend;
using Piranha.Models;
using Piranha.Extend.Fields;
using Catfish.Models.Fields;

namespace Catfish.Models.Regions
{
    public class Footer
    {
        [Field(Title = "Footer logo")]
        public ImageField Logo { get; set; }

        [Field(Title = "Footer Contents")]
        public HtmlField Text { get; set; }

        [Field(Title = "Javascript")]
        public TextField Javascript { get; set; }

        [Field(Title = "Css")]
        public TextField Css { get; set; }

        [Field(Title = "Enabled SubMenu", Placeholder = "If the site will display sub menu")]
        public CheckBoxField EnabledSubMenu { get; set; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Models;
using Piranha.Extend.Fields;
using Catfish.Models.Fields;

namespace Catfish.Models.Regions
{
    public class Header
    {
        //TODO this one doesn't do anything yet
        [Field(Title = "Hide navigation menu", Placeholder = "Remove the submenu at the top of the site")]
        public CheckBoxField RemoveSubMenu { get; set; } = false;
        [Field(Title = "General Header Settings", Placeholder = "Remove the Website Title")]
        public CheckBoxField RemoveWebsiteTitle { get; set; }
        [Field(Title = "General Header Settings", Placeholder = "Remove Default Page Titles")]
        public CheckBoxField RemovePageTitles { get; set; }
        [Field(Title = "Header Background Color", Placeholder = "Please enter a hex value, including the hashtag, or color name")]
        public ColorPicker BackgroundColor { get; set; }
        [Field(Title = "Header Text Color", Placeholder = "Please enter a hex value, including the hashtag, or color name")]
        public ColorPicker TextColor { get; set; }

        [Field(Title = "Header Contents")]
        public HtmlField Text { get; set; }

        [Field(Title = "Javascript")]
        public TextField Javascript { get; set; }

        [Field(Title = "Css")]
        public TextField Css { get; set; }
    }
}

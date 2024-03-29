﻿using CatfishWebExtensions.Models.Attributes;
using CatfishWebExtensions.Models.Sites.Headers;

namespace CatfishWebExtensions.Models.Regions
{
    public class WebSettings
    {
        [Field(Title = "Header Contents")]
        public HtmlField HeaderContents{ get; set; }

        [Field(Title = "Footer Contents")]
        public HtmlField FooterContent { get; set; }

        [Field(Title = "Javascript")]
        public TextField Javascript { get; set; }

        [Field(Title = "Css")]
        public TextField Css { get; set; }

        [Field(Title ="Upload Css File")]
        public Piranha.Extend.Fields.DocumentField CssFile { get; set; }
    }
}

using Catfish.Core.Models;
using Catfish.Services;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Catfish.Core.Services.Solr;
using Catfish.Core.Models.Solr;
using SolrNet;
using Piranha;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Free Search", Category = "Control", Component = "free-search", Icon = "fas fa-search")]
    public class FreeSearchBlock : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<FreeSearchBlock>();

        public TextField CssVal { get; set; }
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

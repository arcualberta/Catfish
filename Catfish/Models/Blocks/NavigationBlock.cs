using Piranha;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Navigation Block", Category = "Content", Component = "navigation-block", Icon = "fas fa-bars")]
    public class NavigationBlock : Block, ICatfishBlock
    {
        public void RegisterBlock() => App.Blocks.Register<NavigationBlock>();

        [Field(Title = "Starting Page Title", Placeholder = "The starting page in the sitemap which other page will be in one group with it")]
        public TextField PageTitle { get; set; }

        public string GetPageTitle()
        {
            if (PageTitle != null)
            {
                return PageTitle.Value;
            }

            return "";
        }
    }
}

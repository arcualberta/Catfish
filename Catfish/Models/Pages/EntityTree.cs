using System;
using System.ComponentModel.Composition;
using Piranha.Extend;
using Catfish.Models.Regions;

namespace Catfish.Models.Pages
{
    [Export(typeof(IPageType))]
    public class EntityTree : PageType
    {
        public EntityTree()
        {
            Name = "Entity Tree";
            Description = "Page for displaying a tree of entity associations";
            Preview = "<table class=\"template\">...</table>";
            Controller = "Entity";

            Regions.Add(new RegionType()
            {
                InternalId = "PageTitle",
                Name = "Page Title",
                Type = typeof(MultilingualText)
            });


        }
    }
}
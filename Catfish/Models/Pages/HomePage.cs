using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.Composition;
using Piranha.Extend;

using Catfish.Models.Widgets;

namespace Catfish.Models.Pages
{
    /// <summary>
    /// The default home page
    /// </summary>
    [Export(typeof(IPageType))]
    public class HomePage : PageType 
    {
        public HomePage()
        {
            Name="Home Page";
            Description = "Default home page which contains an optional carousel and a main HTML body area";
            Preview = @"
                <table class='template'>
                    <tr><td></td></tr>
                    <tr><td></td></tr>
                </table>";
            Controller = "Page";
            View = "HomePage";

            Regions.Add(new RegionType()
            {
                InternalId = "Styles",
                Name = "Styles",
                Type = typeof(Piranha.Extend.Regions.TextRegion)
            });

            Regions.Add(new RegionType()
            {
                InternalId = "Carousel",
                Name = "Carousel",
                Type = typeof(Carousel)
            });

            Regions.Add(new RegionType()
            {
                InternalId = "Content",
                Name = "Content",
                Type = typeof(Piranha.Extend.Regions.HtmlRegion)
            });
        }

    }
}
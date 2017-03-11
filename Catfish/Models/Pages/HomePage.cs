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
    <tr><td>Carousel</td></tr>
    <tr><td>Top Content</td></tr>
    <tr><td style='background:transparent'>
            <table class='template'>
                <tr><td></td><td></td><td></td></tr>
                <tr><td></td><td></td><td></td></tr>
            </table>
    </td></tr>
    <tr><td>Bottom Content</td></tr>
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
                InternalId = "TopContent",
                Name = "Top Content",
                Type = typeof(Piranha.Extend.Regions.HtmlRegion)
            });

            Regions.Add(new RegionType()
            {
                InternalId = "FlexPanel",
                Name = "Flex Panel",
                Type = typeof(FlexBox)
            });

            Regions.Add(new RegionType()
            {
                InternalId = "BottomContent",
                Name = "Bottom Content",
                Type = typeof(Piranha.Extend.Regions.HtmlRegion)
            });
        }

    }
}
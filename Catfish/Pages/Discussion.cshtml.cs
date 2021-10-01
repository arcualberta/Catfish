using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Models.Blocks.TileGrid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class DiscussionModel : PageModel
    {
        [BindProperty]
        public Tile Tile { get; set; }

        public void OnGet(Guid id)
        {
            Tile tile = new Tile();
            tile.Title = "Test Title";
            tile.Subtitle = "Test Subtitle";
            tile.Thumbnail = "https://i.dlpng.com/static/png/6825249_preview.png";
            tile.Content = "AtlasMasland serves designers, architects, " +
                "and businesses who value the positive impact of inspired flooring in every customer experience. " +
                "Atlas Masland has partnered with Chameleon Power to create Click on the image to learn more. " +
                "a room visualizer to help designers, architects, and business clients view new flooring options in their space.";

            Tile = tile;
        }
    }
}
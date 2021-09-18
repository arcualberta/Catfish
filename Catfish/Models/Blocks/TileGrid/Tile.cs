using System;

namespace Catfish.Models.Blocks.TileGrid
{
    public class Tile
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public DateTime Date { get; set; }
        public string ObjectUrl { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Models.Blocks.TileGrid;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TileGridController : ControllerBase
    {
        // GET: api/tilegrid
        [HttpGet]
        public IEnumerable<Tile> Get(int? offset = null, int? max = null)
        {
            List<Tile> tiles = new List<Tile>();

            if (!max.HasValue)
                max = 20;

            if (offset.HasValue && offset.Value > 0)
                max = 10;

            for (int i = 0; i < max; ++i)
            {
                tiles.Add(new Tile()
                {
                    Id = Guid.NewGuid(),
                    Title = "Item " + (i * max + 1),
                    Content = "Content " + (i * max + 1),
                    Date = DateTime.Now.AddDays(i),
                    Thumbnail = "https://www.almanac.com/sites/default/files/styles/primary_image_in_article/public/image_nodes/dahlia-3598551_1920.jpg?itok=XZfJlur2",
                    ObjectUrl = "https://www.almanac.com/plant/dahlias"
                });
            }

            return tiles;
        }

        // GET: api/tilegrid/keywords/block/f8d5815f-ccad-4b72-92ef-51b7c88ea0dd
        [HttpGet]
        [Route("keywords/block/{id:Guid}")]
        public IEnumerable<string> BlcokKeywords(Guid id)
        {
            string[] keywords = new string[] { "keyword 1", "keyword 2", "keyword 3", "keyword 4", "keyword 5", "keyword 6" };

            return keywords;
        }
    }
}

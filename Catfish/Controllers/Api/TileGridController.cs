using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Models.Blocks.TileGrid;
using Piranha.AspNetCore.Services;
using Catfish.Models;
using Piranha.Extend;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TileGridController : ControllerBase
    {
        private readonly IModelLoader _loader;

        public TileGridController(IModelLoader loader)
        {
            _loader = loader;
        }

        // GET: api/tilegrid
        [HttpGet]
        public IEnumerable<Tile> Get(string keywords = null, int? offset = null, int? max = null)
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
                    DetailedViewUrl = "https://www.almanac.com/plant/dahlias"
                });
            }

            return tiles;
        }

        // GET: /api/tilegrid/keywords/page/3c49e3ca-6937-4fa6-ab40-0549f45ca87b/block/3C9C2F8A-C1D8-4869-A8CA-D0641E9200A5
        [HttpGet]
        [Route("keywords/page/{pageId:Guid}/block/{blockId:Guid}")]
        public async Task<IEnumerable<string>> BlcokKeywords(Guid pageId, Guid blockId)
        {
            string[] keywords = Array.Empty<string>();

            var page = await _loader.GetPageAsync<StandardPage>(pageId, HttpContext.User, false).ConfigureAwait(false);

            if(page != null)
            {
                var block = page.Blocks.FirstOrDefault(b => b.Id == blockId);
                if (block != null && (block as Catfish.Models.Blocks.TileGrid.TileGrid).KeywordList != null)
                    keywords = (block as Catfish.Models.Blocks.TileGrid.TileGrid)
                        .KeywordList
                        .Value
                        .Split(new char[] { ',', '\r', '\n' })
                        .Select(v => v.Trim())
                        .Where(v => !string.IsNullOrEmpty(v))
                        .ToArray();
            }
          
            return keywords;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Models.Blocks.TileGrid;
using Piranha.AspNetCore.Services;
using Catfish.Models;
using Piranha.Extend;
using Catfish.Services;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Controllers.Api
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TileGridController : ControllerBase
    {
        private readonly IModelLoader _loader;
        private readonly ISubmissionService _submissionService;
        private readonly AppDbContext _appDb;
        private readonly ISolrService _solr;

        public TileGridController(IModelLoader loader, ISubmissionService submissionService, AppDbContext db, ISolrService solr)
        {
            _loader = loader;
            _submissionService = submissionService;
            _appDb = db;
            _solr = solr;
        }

        // GET: api/tilegrid
        [HttpGet]
        public IEnumerable<Tile> Get(Guid gridId, string keywords = null, int? offset = null, int? max = null)
        {
            //UpdateProperties(gridId, keywords, offset, max);
            string[] sslectedKeywords = string.IsNullOrEmpty(keywords) 
                ? Array.Empty<string>() 
                : keywords.Split('|', StringSplitOptions.RemoveEmptyEntries);

            List<Tile> tiles = new List<Tile>();

            max = 5 * sslectedKeywords.Length;

            for (int i = 0; i < max; ++i)
            {
                tiles.Add(new Tile()
                {
                    Id = Guid.NewGuid(),
                    Title = "Item " + (i * max + 1),
                    Content = Helper.MockHelper.LoremIpsum(4, 8, 1, 5, 1),
                    Date = DateTime.Now.AddDays(i),
                    Thumbnail = "https://www.almanac.com/sites/default/files/styles/primary_image_in_article/public/image_nodes/dahlia-3598551_1920.jpg?itok=XZfJlur2",
                    DetailedViewUrl = "https://www.ualberta.ca/"
                });
            }

            return tiles;
        }

        // GET: /api/tilegrid/keywords/page/3c49e3ca-6937-4fa6-ab40-0549f45ca87b/block/3C9C2F8A-C1D8-4869-A8CA-D0641E9200A5
        [HttpGet]
        [Route("keywords/page/{pageId:Guid}/block/{blockId:Guid}")]
        public async Task<IEnumerable<string>> Keywords(Guid pageId, Guid blockId)
        {
            string[] keywords = Array.Empty<string>();

            var page = await _loader.GetPageAsync<StandardPage>(pageId, HttpContext.User, false).ConfigureAwait(false);

            if (page == null)
                return keywords;

            var block = page.Blocks.FirstOrDefault(b => b.Id == blockId) as TileGrid;
            keywords = block.KeywordList
                .Value
                .Split(new char[] { ',', '\r', '\n' })
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrEmpty(v))
                .ToArray();


            if(!string.IsNullOrEmpty(block.KeywordSourceId.Value))
            {
                Guid? keywordSourceFieldId = Guid.Parse(block.KeywordSourceId.Value);
                Guid selectedItemTemplateId = Guid.Parse(block.SelectedItemTemplate.Value);

                var template = _appDb.ItemTemplates.FirstOrDefault(it => it.Id == selectedItemTemplateId);
                var keywordSourceField = template.GetRootDataItem(false)?.Fields
                    .FirstOrDefault(field => field is OptionsField && field.Id == keywordSourceFieldId)
                    as OptionsField;
                if(keywordSourceField != null)
                {
                    var fieldBasedKeywords = keywordSourceField.Options
                        .SelectMany(opt => opt.OptionText.Values)
                        .Select(txt => txt.Value);

                    keywords = keywords.Union(fieldBasedKeywords).ToArray();
                }
            }

            Array.Sort(keywords);

            return keywords;
        }
    }
}

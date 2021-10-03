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
        public SearchResult Get(Guid gridId, string keywords = null, int offset = 0, int max = 0)
        {
            List<Tile> dbData = GetMockupData();

            //UpdateProperties(gridId, keywords, offset, max);
            string[] slectedKeywords = string.IsNullOrEmpty(keywords) 
                ? Array.Empty<string>() 
                : keywords.Split('|', StringSplitOptions.RemoveEmptyEntries);

            List<Tile> tiles = new List<Tile>();
            if (slectedKeywords.Length > 0)
            {
                foreach (var t in dbData)
                {
                    foreach (var cat in t.Categories)
                    {
                        if (slectedKeywords.Contains(cat))
                        {
                            tiles.Add(t);
                            break;
                        }
                    }
                }
            }
            else
                tiles = dbData;

            SearchResult result = new SearchResult() { Count = tiles.Count };

            if (offset > 0)
                tiles = tiles.Skip(offset).ToList();

            if (max == 0)
                max = 25;
            tiles = tiles.Take(max).ToList();

            result.Items = tiles;
            result.First = offset + 1;
            result.Last = result.First + result.Items.Count() - 1;
            return result;
        }

        private List<Tile> GetMockupData()
        {
            List<Tile> tiles = new List<Tile>();
            int n = 250;
            string[] keywords = new string[] { "Age-appropriate tasks", "Assessment", "Culture", "Games", "Grammar", "Interaction",
                                                "Listening", "Materials", "Reading", "Real-life tasks", "Speaking", "Teacher training",
                                                "Technology", "Vocabulary", "Writing" };

            Random rand = new Random(0);
               
            for (int i = 0; i < n; ++i)
            {
                tiles.Add(new Tile()
                {
                    Id = Guid.NewGuid(),
                    Title = "Item " + (i + 1),
                    Content = Helper.MockHelper.LoremIpsum(4, 8, 1, 5, 1),
                    Date = DateTime.Now.AddDays(i),
                    Thumbnail = "https://www.almanac.com/sites/default/files/styles/primary_image_in_article/public/image_nodes/dahlia-3598551_1920.jpg?itok=XZfJlur2",
                    DetailedViewUrl = "https://www.ualberta.ca/",
                    Categories = rand.Next(0, 2) < 1 
                    ? new string[] {keywords[rand.Next(0, keywords.Length)], keywords[rand.Next(0, keywords.Length)]}
                    : new string[] { keywords[rand.Next(0, keywords.Length)] }
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

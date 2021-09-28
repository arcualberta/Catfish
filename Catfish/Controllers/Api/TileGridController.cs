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


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TileGridController : ControllerBase
    {
        private readonly IModelLoader _loader;
        private readonly ISubmissionService _submissionService;
        private readonly AppDbContext _appDb;

        public TileGridController(IModelLoader loader, ISubmissionService submissionService, AppDbContext db)
        {
            _loader = loader;
            _submissionService = submissionService;
            _appDb = db;
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

            ////MR Sept 22 2021 -- Get all the item pages (in the collection) keywords that selected i "keyword Resource"
            //List<Core.Models.Item> items = _submissionService.GetSubmissionList(User, selectedItemTemplateId, selectedColllectionId); 

            //foreach(Core.Models.Item itm in items)
            //{
            //    DataItem dataItem = itm.GetRootDataItem(false);

            //    var keywordField = dataItem.Fields.Where(f => f.Id == keywordResourceId && typeof(OptionsField).IsAssignableFrom(f.GetType())).FirstOrDefault();//itm.DataContainer.Where(d => d.Fields.Any(f => f.GetType() == typeof(OptionsField) && f.Id == keywordResourceId)).FirstOrDefault();

            //    if (keywordField != null){
                   
            //       string[] resourceKeys = (keywordField as OptionsField).GetSelectedOptionTexts();
            //       keywords = keywords.Union(resourceKeys).ToArray();
            //    }
            //}
          
            return keywords;
        }
    }
}

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
        public async Task<SearchResult> Get(Guid pageId, Guid blockId, string keywords = null, int offset = 0, int max = 0)
        {
            string[] slectedKeywords = string.IsNullOrEmpty(keywords)
               ? Array.Empty<string>()
               : keywords.Split('|', StringSplitOptions.RemoveEmptyEntries);


            SearchResult result = new SearchResult();

            var page = await _loader.GetPageAsync<StandardPage>(pageId, HttpContext.User, false).ConfigureAwait(false);
            if (page == null)
                return result;

            var block = page.Blocks.FirstOrDefault(b => b.Id == blockId) as TileGrid;
            if (block == null)
                return result;

            string collectionId = block.SelectedCollection.Value;
            string solrCollectionFieldName = "collection_s";

            string itemTemplateId = block.SelectedItemTemplate.Value;
            string keywordFieldId = block.KeywordSourceId.Value;
            string dataItemTemplateId = null; //TODO: load the template and get the ID of the root data item
            string solrKeywordFieldName = string.Format("data_{0}_{1}_ts", dataItemTemplateId, keywordFieldId);

            //TODO: Create the solr query, retrieve results and return them wrapped in the
            //Search Result object

            result = Helper.MockHelper.FilterMockupTileGridData(slectedKeywords, offset, max);
            return result;
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

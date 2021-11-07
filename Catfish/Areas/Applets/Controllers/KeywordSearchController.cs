using Catfish.Areas.Applets.Models.KeywordSearch;
using Catfish.Core.Models;
using Catfish.Models;
using Microsoft.AspNetCore.Mvc;
using Piranha.AspNetCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Areas.Applets.Blocks;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Areas.Applets.Models.Solr;
using Catfish.Core.Models.Contents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Areas.Applets.Controllers
{
    //private readonly ISubmissionService _submissionService;
    //private readonly ISolrService _solr;
    //private readonly ErrorLog _errorLog;

    [Route("applets/api/[controller]")]
    [ApiController]
    public class KeywordSearchController : ControllerBase
    {
        private readonly IModelLoader _loader;
        private readonly AppDbContext _appDb;
        public KeywordSearchController(IModelLoader loader, AppDbContext appDb)
        {
            _loader = loader;
            _appDb = appDb;
        }

        // GET: api/<KeywordSearchController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<KeywordSearchController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<KeywordSearchController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<KeywordSearchController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<KeywordSearchController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: /api/tilegrid/keywords/page/3c49e3ca-6937-4fa6-ab40-0549f45ca87b/block/3C9C2F8A-C1D8-4869-A8CA-D0641E9200A5
        [HttpGet]
        [Route("keywords/page/{pageId:Guid}/block/{blockId:Guid}")]
        public async Task<KeywordQueryModel> Keywords(Guid pageId, Guid blockId)
        {
            KeywordQueryModel model = new KeywordQueryModel();

            var page = await _loader.GetPageAsync<StandardPage>(pageId, HttpContext.User, false).ConfigureAwait(false);
            var block = page?.Blocks.FirstOrDefault(b => b.Id == blockId) as KeywordSearch;
            var keywordFieldIdStr = block?.KeywordSourceId?.Value;

            if (!string.IsNullOrEmpty(keywordFieldIdStr))
            {
                //If keywordFieldIdStr is defined, then a template has been selected in the block. Retrieve this templarte.
                Guid selectedItemTemplateId = Guid.Parse(block.SelectedItemTemplate.Value);
                var template = _appDb.ItemTemplates.FirstOrDefault(it => it.Id == selectedItemTemplateId);

                //Retrieve the keyword field from the template
                Guid keywordFieldId = Guid.Parse(keywordFieldIdStr);
                var rootDataItem = template.GetRootDataItem(false);
                var keywordSourceField = rootDataItem?.Fields
                    .FirstOrDefault(field => field.Id == keywordFieldId);
                KeywordFieldContainer keywordFieldContainer = null;
                if (keywordSourceField is OptionsField)
                {
                    //Keyword source is an option field in the root data form. Therefore, we will
                    //take keywords values directly from the list of options in this option field.
                    keywordFieldContainer = new KeywordFieldContainer(
                        rootDataItem, eAggregation.Intersection, eAggregation.Intersection, new Guid[] { keywordSourceField.Id });
                }
                else if (keywordSourceField is FieldContainerReference)
                {
                    //Keyword source is referring to another field container. We will retrieve this field
                    //container depending on whether it refers to a data item or a metadata set and then
                    //build the keyword list by taking all option fields in that container
                    var srcContainerReference = keywordSourceField as FieldContainerReference;
                    var srcContainer = srcContainerReference.RefType == FieldContainerReference.eRefType.metadata
                        ? template.MetadataSets.FirstOrDefault(fc => fc.Id == srcContainerReference.RefId)
                        : template.DataContainer.FirstOrDefault(fc => fc.Id == srcContainerReference.RefId) as FieldContainer;

                    //In this case, we need to get the intersection across fields after getting
                    ////the union of values within a field.
                    keywordFieldContainer = new KeywordFieldContainer(
                        srcContainer, eAggregation.Intersection, eAggregation.Union, null);
                }

                if (keywordFieldContainer != null)
                    model.Containers.Add(keywordFieldContainer);

                model.SortKeywordsInFields();
            }

            return model;
        }
    }
}

using Catfish.Areas.Applets.Models.Blocks;
using Catfish.Core.Models;
using Catfish.Models;
using Microsoft.AspNetCore.Mvc;
using Piranha.AspNetCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Areas.Applets.Models.Blocks.KeywordSearchModels;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Areas.Applets.Models.Solr;
using Catfish.Core.Models.Contents;
using Newtonsoft.Json;
using Catfish.Core.Services;
using ElmahCore;
using Catfish.Core.Models.Solr;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Areas.Applets.Controllers
{
    //private readonly ISubmissionService _submissionService;

    [Route("applets/api/[controller]")]
    [ApiController]
    public class KeywordSearchController : ControllerBase
    {
        private readonly IModelLoader _loader;
        private readonly AppDbContext _appDb;
        private readonly ISolrService _solr;
        private readonly ErrorLog _errorLog;
        public KeywordSearchController(IModelLoader loader, AppDbContext appDb, ISolrService solr, ErrorLog errorLog)
        {
            _loader = loader;
            _appDb = appDb;
            _solr = solr;
            _errorLog = errorLog;
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

                //model.SortKeywordsInFields();
            }

            return model;
        }


        [HttpPost]
        [Route("items")]
        public async Task<SearchOutput> GetItems([FromForm] Guid pageId, [FromForm] Guid blockId, [FromForm] string queryParams, [FromForm] int offset = 0, [FromForm] int max = 0)
        {
            //Using mockup data
            KeywordQueryModel qModel = await Keywords(pageId, blockId).ConfigureAwait(false);
            DataMockupHelpers.KeywordSearchMockupHelper helper = new DataMockupHelpers.KeywordSearchMockupHelper(qModel.Containers, 250);
            return helper.FilterMockupData(JsonConvert.DeserializeObject<KeywordQueryModel>(queryParams), offset, max);


            SearchOutput result = new SearchOutput();
            try
            {

                var page = await _loader.GetPageAsync<StandardPage>(pageId, HttpContext.User, false).ConfigureAwait(false);
                if (page == null)
                    return result;

                var block = page.Blocks.FirstOrDefault(b => b.Id == blockId) as KeywordSearch;
                if (block == null)
                    return result;


                string collectionId = block.SelectedCollection.Value;
                string solrCollectionFieldName = "collection_s";

                string itemTemplateId = block.SelectedItemTemplate.Value;
                string keywordFieldId = block.KeywordSourceId.Value;
                string dataItemTemplateId = null; //TODO: load the template and get the ID of the root data item
                string solrKeywordFieldName = string.Format("data_{0}_{1}_ts", dataItemTemplateId, keywordFieldId);
                string detailedViewUrl = block.DetailedViewUrl.Value?.TrimEnd('/') + "/";


                KeywordQueryModel keywordQueryModel = JsonConvert.DeserializeObject<KeywordQueryModel>(queryParams);

                string keywords = null;
                string[] slectedKeywords = string.IsNullOrEmpty(keywords)
                   ? Array.Empty<string>()
                   : keywords.Split('|', StringSplitOptions.RemoveEmptyEntries);

                var query = keywordQueryModel?.BuildSolrQuery();
                string scope = string.Format("collection_s:{0} AND doc_type_ss:item", collectionId);
                query = string.IsNullOrEmpty(query)
                    ? scope
                    : string.Format("{0} AND {1}", scope, query);

                // System.IO.File.WriteAllText("c:\\Temp\\solr_query.txt", query);

                SearchResult solrSearchResult = _solr.ExecuteSearch(query, offset, max, 10);

                //Wrapping the results in the SearchOutput object
                Guid titleFieldId = string.IsNullOrEmpty(block.SelectedMapTitleId.Value) ? Guid.Empty : Guid.Parse(block.SelectedMapTitleId.Value);
                Guid subtitleFieldId = string.IsNullOrEmpty(block.SelectedMapSubtitleId.Value) ? Guid.Empty : Guid.Parse(block.SelectedMapSubtitleId.Value);
                Guid contentFieldId = string.IsNullOrEmpty(block.SelectedMapContentId.Value) ? Guid.Empty : Guid.Parse(block.SelectedMapContentId.Value);
                Guid thumbnailFieldId = string.IsNullOrEmpty(block.SelectedMapThumbnailId.Value) ? Guid.Empty : Guid.Parse(block.SelectedMapThumbnailId.Value);
                Guid keywordsFieldId = string.IsNullOrEmpty(block.KeywordSourceId.Value) ? Guid.Empty : Guid.Parse(block.KeywordSourceId.Value);

                foreach (var resultEntry in solrSearchResult.ResultEntries)
                {
                    ResultItem resultItem = new ResultItem();
                    resultItem.Id = resultEntry.Id;
                    resultItem.Title = Combine(resultEntry.Fields.FirstOrDefault(field => field.FieldId == titleFieldId)?.FieldContent);
                    resultItem.Subtitle = Combine(resultEntry.Fields.FirstOrDefault(field => field.FieldId == subtitleFieldId)?.FieldContent);
                    resultItem.Content = Combine(resultEntry.Fields.FirstOrDefault(field => field.FieldId == contentFieldId)?.FieldContent);
                    resultItem.Thumbnail = Combine(resultEntry.Fields.FirstOrDefault(field => field.FieldId == thumbnailFieldId)?.FieldContent);
                    resultItem.DetailedViewUrl = detailedViewUrl + resultEntry.Id;

                    var categories = resultEntry.Fields.FirstOrDefault(field => field.FieldId == keywordsFieldId)?.FieldContent.ToArray();
                    if (keywordsFieldId != Guid.Empty && categories == null)
                        _errorLog.Log(new Error(new Exception(string.Format("Keyword field with ID {0} not found for item with ID {1}", keywordsFieldId, resultEntry.Id))));
                    else
                    {
                        foreach (var cat in categories)
                        {
                            if (cat.StartsWith("ref://"))
                            {
                                try
                                {


                                    //This is a reference field
                                    var parts = cat.Substring(6).Split("_");
                                    var containerId = Guid.Parse(parts[1]);

                                    //Get all fields that starts with the prefix
                                    var keywordFields = resultEntry.Fields.Where(field => field.ContainerId == containerId).ToList();
                                    foreach (var kf in keywordFields)
                                        resultItem.Categories.AddRange(kf.FieldContent);
                                }
                                catch (Exception ex)
                                {
                                    _errorLog.Log(new Error() { Message = "Field referencing error." });
                                    _errorLog.Log(new Error(ex));
                                }
                            }
                            else
                                resultItem.Categories.Add(cat);
                        }
                    }

                    result.Items.Add(resultItem);
                }
                result.First = solrSearchResult.Offset;
                result.Count = solrSearchResult.TotalMatches;

                //result = Helper.MockHelper.FilterMockupTileGridData(slectedKeywords, offset, max);

            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
            return result;
        }

        private string Combine(List<string> components)
        {
            return components == null ? null : string.Join(" / ", components);
        }


    }
}

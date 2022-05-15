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
using Piranha.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Cors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Areas.Applets.Controllers
{
    //private readonly ISubmissionService _submissionService;

    [Route("applets/api/[controller]")]
    [ApiController]
    [EnableCors("CatfishApiPolicy")]
    public class KeywordSearchController : ControllerBase
    {
        private readonly IModelLoader _loader;
        private readonly AppDbContext _appDb;
        private readonly Piranha.AspNetCore.Identity.IDb _piranhaDb;
        private readonly ISolrService _solr;
        private readonly ErrorLog _errorLog;
        public KeywordSearchController(IModelLoader loader, AppDbContext appDb, Piranha.AspNetCore.Identity.IDb piranhaDb, ISolrService solr, ErrorLog errorLog)
        {
            _loader = loader;
            _appDb = appDb;
            _piranhaDb = piranhaDb;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="collectionId"></param>
        /// <param name="groupId"></param>
        /// <param name="permissibleStateGuids">These are the IDs of the status values to be considerd for the result set irrespective of the current user's permissions.</param>
        /// <param name="queryParams"></param>
        /// <param name="searchText"></param>
        /// <param name="offset"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [HttpPost]
        public SearchOutput Post([FromForm] Guid templateId, [FromForm] Guid collectionId, [FromForm] Guid groupId, [FromForm] Guid[] stateIdRestrictions, [FromForm] string queryParams, [FromForm] string searchText = null, [FromForm] int offset = 0, [FromForm] int max = 0)
        {
           // Dictionary<string, object> result = new Dictionary<string, object>();
            SearchOutput result = new SearchOutput();
            try
            {
                #region Validating access-permission for the currently logged in user

                ItemTemplate template = _appDb.ItemTemplates.FirstOrDefault(t => t.Id == templateId);

                //Taking the subset of state IDs from the list of permissible state IDs such that the selected subset would be the 
                //list of permitted states for the user who is currently logged in(if any) based on the user's role withing the specified group.
                List<Guid> permittedStatusIds = null;
                if (stateIdRestrictions.Length > 0)
                {
                    permittedStatusIds = GetPermittedStateIdsForCurrentUser(groupId, template, "ListInstances", stateIdRestrictions);

                    if (permittedStatusIds.Count == 0)
                        return result;
                }
                #endregion

                KeywordQueryModel keywordQueryModel = JsonConvert.DeserializeObject<KeywordQueryModel>(queryParams);

                string keywords = null;
                string[] slectedKeywords = string.IsNullOrEmpty(keywords)
                   ? Array.Empty<string>()
                   : keywords.Split('|', StringSplitOptions.RemoveEmptyEntries);

                var query = keywordQueryModel?.BuildSolrQuery();
                string scope = string.Format("doc_type_ss:item AND collection_s:{0} AND template_s:{1}", collectionId, templateId);
                query = string.IsNullOrEmpty(query)
                        ? scope
                        : string.Format("{0} AND {1}", scope, query);


                if (groupId != null && groupId != Guid.Empty)
                    query = string.Format("{0} AND group_s:{1}", query, groupId.ToString());

                if (stateIdRestrictions.Length > 0)
                {
                    List<string> stateLimits = new List<string>();
                    foreach (var stId in permittedStatusIds)
                        stateLimits.Add(string.Format("status_s:{0}", stId));
                    query = string.Format("{0} AND ({1})", query, string.Join(" OR ", stateLimits));
                }


                SearchResult solrSearchResult = _solr.ExecuteSearch(query, offset, max, 10, searchText);

                foreach (ResultEntry resultEntry in solrSearchResult.ResultEntries)
                {
                    ResultItem resultItem = new ResultItem();
                    resultItem.Id = resultEntry.Id;
                    resultItem.Date = resultEntry.Created;
                    string solrFieldId = "";
                    foreach(var field in resultEntry.Fields)
                    {
                        if (!string.IsNullOrEmpty(field.Scope.ToString())) {
                            solrFieldId = field.FieldKey; 
                            
                            resultItem.SolrFields.Add(field.FieldKey, field.FieldContent.ToArray());
                        }
                    }

                    result.Items.Add(resultItem);
                }
                result.First = solrSearchResult.Offset + 1;
                result.Count = solrSearchResult.TotalMatches;
                result.Last = result.First + result.Items.Count - 1;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }

           return result;
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
        public async Task<SearchOutput> GetItems([FromForm] Guid pageId, [FromForm] Guid blockId, [FromForm] string queryParams, [FromForm] string searchText = null, [FromForm] int offset = 0, [FromForm] int max = 0)
        {
            //////Using mockup data
            ////KeywordQueryModel qModel = await Keywords(pageId, blockId).ConfigureAwait(false);
            ////DataMockupHelpers.KeywordSearchMockupHelper helper = new DataMockupHelpers.KeywordSearchMockupHelper(qModel.Containers, 250);
            ////return helper.FilterMockupData(JsonConvert.DeserializeObject<KeywordQueryModel>(queryParams), offset, max);

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

                Guid itemTemplateId = Guid.Parse(block.SelectedItemTemplate.Value);
                string keywordFieldId = block.KeywordSourceId.Value;
                string detailedViewUrl = block.DetailedViewUrl.Value?.TrimEnd('/');
                Guid? groupId = string.IsNullOrEmpty(block.SelectedGroupId.Value) ? null as Guid? : Guid.Parse(block.SelectedGroupId.Value);


                #region Validating access-permission for the currently logged in user

                ItemTemplate template = _appDb.ItemTemplates.FirstOrDefault(t => t.Id == itemTemplateId);

                //Take the permissible state GUIDs from the Piranha bloclk (i.e. GUIDs of selected states)
                var permissibleStateGuids = block.GetSelectedStates();

                var permittedStatusIds = GetPermittedStateIdsForCurrentUser(Guid.Parse(block.SelectedGroupId.Value), template, "ListInstances", permissibleStateGuids);

                if (permittedStatusIds.Count == 0)
                    return result;

                #endregion



                KeywordQueryModel keywordQueryModel = JsonConvert.DeserializeObject<KeywordQueryModel>(queryParams);

                string keywords = null;
                string[] slectedKeywords = string.IsNullOrEmpty(keywords)
                   ? Array.Empty<string>()
                   : keywords.Split('|', StringSplitOptions.RemoveEmptyEntries);

                var query = keywordQueryModel?.BuildSolrQuery();
                string scope = string.Format("doc_type_ss:item AND collection_s:{0} AND template_s:{1}", collectionId, itemTemplateId);
                query = string.IsNullOrEmpty(query)
                        ? scope
                        : string.Format("{0} AND {1}", scope, query);
            

                ////if (groupId.HasValue)
                ////    query = string.Format("{0} AND group_s:{1}", query, groupId.Value);

                List<string> stateLimits = new List<string>();
                foreach (var stId in permittedStatusIds)
                    stateLimits.Add(string.Format("status_s:{0}", stId));
                query = string.Format("{0} AND ({1})", query, string.Join(" OR ", stateLimits));

                //System.IO.File.WriteAllText("c:\\solr_query.txt", query);

                //April 27 2022 -- add seachText parameter 
                SearchResult solrSearchResult = _solr.ExecuteSearch(query, offset, max, 10, searchText);

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
                    resultItem.Date = resultEntry.Created;
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
                result.First = solrSearchResult.Offset + 1;
                result.Count = solrSearchResult.TotalMatches;
                result.Last = result.First + result.Items.Count - 1;
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

        private List<Guid> GetPermittedStateIdsForCurrentUser(
            Guid groupId,
            ItemTemplate template, 
            string actionFunction, 
            Guid[] permissibleStateIds)
        {

            //Get the listing action from the workflow template
            var action = template.Workflow.Actions.FirstOrDefault(action => action.Function == actionFunction);
            if (action == null)
                return new List<Guid>(); //Action with requested function does not exist, so returns an empty array.

            //Get the states of the selected action, excluding the states that were requested to be excluded
            var actionStateRefs = action.States.Where(st => permissibleStateIds.Contains(st.RefId));

            
            if (User == null || string.IsNullOrEmpty(User?.Identity?.Name))
            {
                //Return the states of the item where the public can perform the specified acton.
                return actionStateRefs.Where(st => st.IsPublic).Select(st => st.RefId).ToList();
            }
            else if (User.IsInRole("SysAdmin"))
			{
                //Grant access to SysAdmin users
                //Return all the non-excluded states
                return actionStateRefs.Select(st => st.RefId).ToList();
            }
            else 
            {
                List<Guid> permittedStateGuids = new List<Guid>();

                //Check if the current user holds the Member role within the TBLT group and if so grant access
                Guid? tbltGroupId = _appDb.Groups
                    .Where(g => g.Id == groupId)
                    .Select(g => g.Id)
                    .FirstOrDefault();
                if (!tbltGroupId.HasValue)
                    throw new Exception(string.Format("No {0} group found", groupId));

                //Get user
                User loginUser = _piranhaDb.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                Guid userId = loginUser.Id;

                //Select the list of roles where the current user is associated within the group.
                var userRoleIdsWithinGroup = _appDb.UserGroupRoles
                    .Where(ugr => ugr.UserId == userId)
                    .Select(ugr => ugr.GroupRole.RoleId)
                    .ToList();

                //Iterate through all action states and select the subset of states where any of the above roleIds are authorized
                foreach (var stateRef in actionStateRefs)
                {
                    if(stateRef.AuthorizedRoles.Where(ar => userRoleIdsWithinGroup.Contains(ar.RefId)).Any())
                        permittedStateGuids.Add(stateRef.RefId);          
                }

                return permittedStateGuids;
            }
        }

    }
}

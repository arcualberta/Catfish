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
using Catfish.Models.Blocks.TileGrid.Keywords;
using Catfish.Core.Models.Contents;
using Newtonsoft.Json;
using Catfish.Core.Models.Solr;
using ElmahCore;
using Piranha;
using Microsoft.AspNetCore.Identity;
using Piranha.AspNetCore.Identity.Data;


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
        private readonly ErrorLog _errorLog;
        private readonly Piranha.AspNetCore.Identity.IDb _db;
        public TileGridController(IModelLoader loader, ISubmissionService submissionService, AppDbContext db, ISolrService solr, ErrorLog errorLog, Piranha.AspNetCore.Identity.IDb piranhaDb)
        {
            _loader = loader;
            _submissionService = submissionService;
            _appDb = db;
            _solr = solr;
            _errorLog = errorLog;
           
            _db = piranhaDb;
        }

        // GET: api/tilegrid
        [HttpPost]
        //[Route("items/page/{pageId:Guid}/block/{blockId:Guid}")]
        [Route("items")]
        public async Task<SearchOutput> Get([FromForm] Guid pageId, [FromForm] Guid blockId, [FromForm] string queryParams, [FromForm] int offset = 0, [FromForm] int max = 0)
        {
            SearchOutput result = new SearchOutput();
            try
            {

                //Grant access to SysAdmin users
                bool accessPermitted = User!= null && User.IsInRole("SysAdmin");

                if (!accessPermitted && !string.IsNullOrEmpty(User?.Identity?.Name))
                {
                    //Check if the current user holds the Member role within the TBLT group and if so grant access

                   //1. Get "Member" role
                    Guid? memberRoleId = Guid.NewGuid();
                  
                    Role MemberRole = _db.Roles.Where(r => r.Name == "Member").FirstOrDefault();
                    if (MemberRole == null)
                    {
                        throw new Exception("No Member role found");
                    }
                    if (MemberRole != null)
                        memberRoleId = MemberRole.Id;


                    //Get user
                    User loginUser = _db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault(); 
                    Guid userId = new Guid();
                    if (loginUser != null)
                        userId = loginUser.Id;

                    Guid? tbltGroupId = _appDb.Groups
                        .Where(g => g.Name.ToLower() == "tblt")
                        .Select(g => g.Id)
                        .FirstOrDefault();
                    if (!tbltGroupId.HasValue)
                        throw new Exception("No TBLT group found");


                    Guid? tbltMemberGroupRoleId = _appDb.GroupRoles
                        .Where(gr => gr.GroupId == tbltGroupId && gr.RoleId == memberRoleId)
                        .Select(gr => gr.Id)
                        .FirstOrDefault();
                    if (!tbltMemberGroupRoleId.HasValue)
                        throw new Exception("Member role is not associated with the TBLT group");

                    Guid? userMemberRoleWithinTblt = _appDb.UserGroupRoles.
                        Where(ugr => ugr.UserId == userId && ugr.GroupRoleId == tbltMemberGroupRoleId)
                        .Select(ugr => ugr.Id)
                        .FirstOrDefault();
                    accessPermitted = userMemberRoleWithinTblt.HasValue;
                }

                if (!accessPermitted)
                    return result;

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
                string detailedViewUrl = block.DetailedViewUrl.Value;

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

                Guid? approvedStateId = _appDb.SystemStatuses
                    .Where(s => s.NormalizedStatus == "APPROVED" && s.EntityTemplateId == Guid.Parse(itemTemplateId))
                    .Select(s => s.Id)
                    .FirstOrDefault();
                if(approvedStateId.HasValue)
                    query = string.Format("{0} AND status_s:{1}", query, approvedStateId.Value);

                // System.IO.File.WriteAllText("c:\\Temp\\solr_query.txt", query);

                SearchResult solrSearchResult = _solr.ExecuteSearch(query, offset, max, 10);

                //Wrapping the results in the SearchOutput object
                Guid titleFieldId = string.IsNullOrEmpty(block.SelectedMapTitleId.Value) ? Guid.Empty : Guid.Parse(block.SelectedMapTitleId.Value);
                Guid subtitleFieldId = string.IsNullOrEmpty(block.SelectedMapSubtitleId.Value) ? Guid.Empty : Guid.Parse(block.SelectedMapSubtitleId.Value);
                Guid contentFieldId = string.IsNullOrEmpty(block.SelectedMapContentId.Value) ? Guid.Empty : Guid.Parse(block.SelectedMapContentId.Value);
                Guid thumbnailFieldId = string.IsNullOrEmpty(block.SelectedMapThumbnailId.Value) ? Guid.Empty : Guid.Parse(block.SelectedMapThumbnailId.Value);
                Guid keywordsFieldId = string.IsNullOrEmpty(block.KeywordSourceId.Value) ? Guid.Empty : Guid.Parse(block.KeywordSourceId.Value);
                
                foreach (var resultItem in solrSearchResult.ResultEntries)
                {
                    Tile tile = new Tile();
                    tile.Id = resultItem.Id;
                    tile.Title = Combine(resultItem.Fields.FirstOrDefault(field => field.FieldId == titleFieldId)?.FieldContent);
                    tile.Subtitle = Combine(resultItem.Fields.FirstOrDefault(field => field.FieldId == subtitleFieldId)?.FieldContent);
                    tile.Content = Combine(resultItem.Fields.FirstOrDefault(field => field.FieldId == contentFieldId)?.FieldContent);
                    tile.Thumbnail = Combine(resultItem.Fields.FirstOrDefault(field => field.FieldId == thumbnailFieldId)?.FieldContent);
                    tile.DetailedViewUrl = detailedViewUrl + resultItem.Id;

                    var categories = resultItem.Fields.FirstOrDefault(field => field.FieldId == keywordsFieldId)?.FieldContent.ToArray();
                    if (keywordsFieldId != Guid.Empty && categories == null)
                        _errorLog.Log(new Error(new Exception(string.Format("Keyword field with ID {0} not found for item with ID {1}", keywordsFieldId, resultItem.Id))));
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
                                    var keywordFields = resultItem.Fields.Where(field => field.ContainerId == containerId).ToList();
                                    foreach (var kf in keywordFields)
                                        tile.Categories.AddRange(kf.FieldContent);
                                }
                                catch (Exception ex)
                                {
                                    _errorLog.Log(new Error() { Message = "Field referencing error." });
                                    _errorLog.Log(new Error(ex));
                                }
                            }
                            else
                                tile.Categories.Add(cat);
                        }
                    }

                    result.Items.Add(tile);
                }

                result.First = solrSearchResult.Offset + 1;
                result.Count = solrSearchResult.TotalMatches;
                result.Last = result.First + result.Items.Count - 1;
            }
            catch(Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
            return result;
        }

        private string Combine(List<string> components)
        {
            return components == null ? null : string.Join(" / ", components);
        }


        // GET: /api/tilegrid/keywords/page/3c49e3ca-6937-4fa6-ab40-0549f45ca87b/block/3C9C2F8A-C1D8-4869-A8CA-D0641E9200A5
        [HttpGet]
        [Route("keywords/page/{pageId:Guid}/block/{blockId:Guid}")]
        public async Task<KeywordQueryModel> Keywords(Guid pageId, Guid blockId)
        {
            KeywordQueryModel model = new KeywordQueryModel();

            var page = await _loader.GetPageAsync<StandardPage>(pageId, HttpContext.User, false).ConfigureAwait(false);
            var block = page?.Blocks.FirstOrDefault(b => b.Id == blockId) as TileGrid;
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

        //public async Task<IEnumerable<string>> Keywords(Guid pageId, Guid blockId)
        //{
        //    string[] keywords = Array.Empty<string>();

        //    var page = await _loader.GetPageAsync<StandardPage>(pageId, HttpContext.User, false).ConfigureAwait(false);

        //    if (page == null)
        //        return keywords;

        //    var block = page.Blocks.FirstOrDefault(b => b.Id == blockId) as TileGrid;
        //    keywords = block?.KeywordList?
        //        .Value?
        //        .Split(new char[] { ',', '\r', '\n' })
        //        .Select(v => v.Trim())
        //        .Where(v => !string.IsNullOrEmpty(v))
        //        .ToArray();


        //    if(!string.IsNullOrEmpty(block?.KeywordSourceId?.Value))
        //    {
        //        Guid? keywordSourceFieldId = Guid.Parse(block.KeywordSourceId.Value);
        //        Guid selectedItemTemplateId = Guid.Parse(block.SelectedItemTemplate.Value);

        //        var template = _appDb.ItemTemplates.FirstOrDefault(it => it.Id == selectedItemTemplateId);
        //        var keywordSourceField = template.GetRootDataItem(false)?.Fields
        //            .FirstOrDefault(field => field is OptionsField && field.Id == keywordSourceFieldId)
        //            as OptionsField;
        //        if(keywordSourceField != null)
        //        {
        //            var fieldBasedKeywords = keywordSourceField.Options
        //                .SelectMany(opt => opt.OptionText.Values)
        //                .Select(txt => txt.Value);

        //            keywords = keywords.Union(fieldBasedKeywords).ToArray();
        //        }
        //    }

        //    Array.Sort(keywords);

        //    return keywords;
        //}
    }
}

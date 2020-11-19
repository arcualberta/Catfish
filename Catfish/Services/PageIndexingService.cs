using Catfish.Core.Models.Solr;
using Catfish.Core.Services.Solr;
using Piranha;
using Piranha.Models;
using Piranha.Extend;
using Piranha.Extend.Blocks;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Models.Blocks;
using Microsoft.AspNetCore.Http;
using ElmahCore;
using Hangfire.Logging.LogProviders;
using Catfish.Models.Regions;
using Catfish.Models;
using Catfish.Models.Fields;
using Piranha.AspNetCore.Identity.SQLServer;
using Catfish.Models.SiteTypes;

namespace Catfish.Services
{
    public class PageIndexingService : IPageIndexingService
    {
        private readonly IApi _api;
        private readonly IdentitySQLServerDb _piranhaDb;
        private readonly ISolrIndexService<SolrEntry> _solrIndexService;
        private readonly IQueryService _solrQueryService;
        private readonly ErrorLog _errorLog;
        private readonly ICatfishSiteService _catfishSiteService;

        public PageIndexingService(ISolrIndexService<SolrEntry> iSrv, IQueryService qSrv, ICatfishSiteService sSrv, IApi api, IdentitySQLServerDb pdb, ErrorLog errorLog)
        {
            _api = api;
            _solrIndexService = iSrv;
            _solrQueryService = qSrv;
            _errorLog = errorLog;
            _piranhaDb = pdb;
            _catfishSiteService = sSrv;
        }

        protected void IndexBlock(Block block, SolrEntry entry)
        {
            if (block == null || entry == null)
                return;

            //If the given block is an HtmlBlock or any specialization of it,
            //then we index its body content
            if (typeof(HtmlBlock).IsAssignableFrom(block.GetType()))
            {
                string text = (block as HtmlBlock).Body.Value;
                if (!string.IsNullOrWhiteSpace(text))
                    entry.AddContent(block.Id, text);
            }

            //If the given block is an TextBlock or any specialization of it,
            //then we index its body content
            if (typeof(TextBlock).IsAssignableFrom(block.GetType()))
            {
                string text = (block as TextBlock).Body.Value;
                if (!string.IsNullOrWhiteSpace(text))
                    entry.AddContent(block.Id, text);
            }

            //If the given block is an QuoteBlock or any specialization of it,
            //then we index its body content
            if (typeof(QuoteBlock).IsAssignableFrom(block.GetType()))
            {
                string text = (block as QuoteBlock).Body.Value;
                if (!string.IsNullOrWhiteSpace(text))
                    entry.AddContent(block.Id, text);
            }

            //For given block is an ImageBlock or any specialization of it,
            //then we index its Url  
            //  - need to remove leading tilde for now
            if (typeof(ImageBlock).IsAssignableFrom(block.GetType()))
            {
                string text = (block as ImageBlock).Body.Media.PublicUrl;
                if (!string.IsNullOrWhiteSpace(text))
                    entry.AddImage(block.Id, text.TrimStart('~'));
            }


            //If the given block is an ColumnBlock or any specialization of it,
            //then we index each block inside it
            if (typeof(ColumnBlock).IsAssignableFrom(block.GetType()))
            {
                var children = (block as ColumnBlock).Items;
                foreach (var child in children)
                    IndexBlock(child, entry);
            }
        }

        public void IndexPage(PageBase doc)
        {
            try
            {
                if (doc == null || !doc.IsPublished)
                    return;

                SolrEntry entry = new SolrEntry()
                {
                    Id = doc.Id,
                    ObjectType = SolrEntry.eEntryType.Page,
                    Permalink = string.IsNullOrWhiteSpace(doc.Permalink) ? null : doc.Permalink,
                };

                entry.SetTitle(doc.Id, doc.Title);

                //Index any keywords selected for the page
                List<string> keywords = new List<string>();
                try
                {
                    var selectedOptionStr = ((doc as DynamicPage).Regions.Keywords as ControlledKeywordsField)
                        .SelectedKeywords
                        .Value;

                    if (!string.IsNullOrWhiteSpace(selectedOptionStr))
                    {
                        keywords = selectedOptionStr
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim())
                            .ToList();

                        foreach (var kw in keywords)
                            entry.Keywords.Add(kw);
                    }
                }
                catch (Exception ex)
                {
                    _errorLog.Log(new Error(ex));
                }

                //Index any categories selected for the page
                List<string> categories = new List<string>();
                try
                {
                    var selectedOptionStr = ((doc as DynamicPage).Regions.Categories as ControlledCategoriesField)
                        .SelectedCategories
                        .Value;

                    if (!string.IsNullOrWhiteSpace(selectedOptionStr))
                    {
                        categories = selectedOptionStr
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Trim())
                            .ToList();

                        foreach (var cw in categories)
                            entry.Categories.Add(cw);
                    }
                }
                catch (Exception ex)
                {
                    _errorLog.Log(new Error(ex));
                }


                if (!string.IsNullOrEmpty(doc.Excerpt))
                    entry.AddContent(doc.Id, doc.Excerpt);

                //Indexing all content blocks
                foreach (var block in doc.Blocks)
                    IndexBlock(block, entry);

                IndexInSolr(entry);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
        }

        public void IndexPost(PostBase doc)
        {
            try
            {
                if (doc == null || !doc.IsPublished)
                return;

                SolrEntry entry = new SolrEntry()
                {
                    Id = doc.Id,
                    ObjectType = SolrEntry.eEntryType.Post,
                    Permalink = string.IsNullOrWhiteSpace(doc.Permalink) ? null : doc.Permalink,
                };

                entry.SetTitle(doc.Id, doc.Title);

                //Index any keywords selected for the page
                List<string> keywords = new List<string>();
                try
                {
                    keywords = ((doc as DynamicPost).Regions.Keywords as ControlledKeywordsField)
                        .SelectedKeywords
                        .Value
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    foreach (var kw in keywords)
                        entry.Keywords.Add(kw);
                }
                catch (Exception ex)
                {
                    _errorLog.Log(new Error(ex));
                }

                //Index any categories selected for the page
                List<string> Categories = new List<string>();
                try
                {
                    Categories = ((doc as DynamicPost).Regions.Categories as ControlledCategoriesField)
                        .SelectedCategories
                        .Value
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    foreach (var cw in Categories)
                        entry.Categories.Add(cw);
                }
                catch (Exception ex)
                {
                    _errorLog.Log(new Error(ex));
                }

                if (!string.IsNullOrEmpty(doc.Excerpt))
                    entry.AddContent(doc.Id, doc.Excerpt);

                //Indexing all content blocks
                foreach (var block in doc.Blocks)
                    IndexBlock(block, entry);

                IndexInSolr(entry);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
        }
        private void IndexInSolr(SolrEntry entry)
        {
            _solrIndexService.AddUpdate(entry);
        }

        public async Task<List<Site>> GetSitesList()
        {
            try
            {
                List<Site> sites = new List<Site>();
                var siteList =await _api.Sites.GetAllAsync().ConfigureAwait(false);
                foreach (var site in siteList)
                {
                    sites.Add(site);
                }
                return sites;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public bool IndexSite(Guid siteId, string siteType)
        {
            try
            {
                if (siteType == typeof(CatfishWebsite).Name || siteType == typeof(WorkflowPortal).Name)
                {
                    //Get all the pages of the site and update their keywords
                    var pages = _api.Pages.GetAllAsync(siteId).ConfigureAwait(false).GetAwaiter().GetResult();
                    foreach (var page in pages)
                        IndexPage(page);

                    //Get all posts of the site and update their keywords
                    var posts = _api.Posts.GetAllAsync(siteId).ConfigureAwait(false).GetAwaiter().GetResult();
                    foreach (var post in posts)
                        IndexPost(post);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return false;
            }
        }
    }
}

using Catfish.Core.Models;
using Catfish.Models;
using Catfish.Models.Blocks;
using Catfish.Models.Fields;
using Catfish.Models.SiteTypes;
using ElmahCore;
using Piranha;
using Piranha.Models;
using Piranha.Services;
using SolrNet.Mapping.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class CatfishSiteService : ICatfishSiteService
    {
        private IApi _api;
        private IApi api;
        private AppDbContext db;
        private ErrorLog errorLog;
        private readonly ErrorLog _errorLog;
        private AppDbContext _db { get; set; }
        public CatfishSiteService(IApi api, ErrorLog errorLog, AppDbContext db)
        {
            _api = api;
            _errorLog = errorLog;
            _db = db;
        }

        public CatfishSiteService(IApi api, AppDbContext db, ErrorLog errorLog)
        {
            this.api = api;
            this.db = db;
            this.errorLog = errorLog;
        }

        public async Task UpdateKeywordVocabularyAsync(SiteContentBase siteContentBase)
        {
            try
            {
                Guid siteId = siteContentBase.Id;
                var siteTypeId = siteContentBase.TypeId;

                if (siteTypeId == typeof(CatfishWebsite).Name || siteTypeId == typeof(WorkflowPortal).Name)
                {
                    //Get the contents of the given site
                    var siteContent = await _api.Sites.GetContentByIdAsync(siteId).ConfigureAwait(false);

                    //Gets the keywords field of the site settings
                    var keywordsField = siteContent.Regions.Keywords as Piranha.Extend.Fields.TextField;

                    //Keywords
                    var keywords = keywordsField.Value.Split(new char[] { ',', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    string concatenatedKeywords = string.Join(",", keywords);

                    //categories
                    //OCt 30 2020 - Gets the categories field of the site settings
                    var categoriesField = siteContent.Regions.Categories as Piranha.Extend.Fields.TextField;
                    var categories = categoriesField.Value.Split(new char[] { ',', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    string concatenatedCategories = string.Join(",", keywords);


                    //Get all the pages of the site and update their keywords
                    var pages = await _api.Pages.GetAllAsync(siteId).ConfigureAwait(false);
                    foreach (var page in pages)
                    {
                        try
                        {
                            UpdateKeywordVocabularyAsync(page, concatenatedKeywords, concatenatedCategories);
                            await _api.Pages.SaveAsync<DynamicPage>(page).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            _errorLog.Log(new Error(ex));
                        }
                    }

                    //Get all posts of the site and update their keywords
                    var posts = await _api.Posts.GetAllAsync(siteId).ConfigureAwait(false);
                    foreach (var post in posts)
                    {
                        try
                        {
                            var postKeywords = post.Regions.Keywords as ControlledKeywordsField;
                            postKeywords.Vocabulary.Value = concatenatedKeywords;
                            UpdateKeywordVocabularyAsync(post, concatenatedKeywords, concatenatedCategories);
                            await _api.Posts.SaveAsync<DynamicPost>(post).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            _errorLog.Log(new Error(ex));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }

        }

        public void UpdateKeywordVocabularyAsync(DynamicPage page, string concatenatedVocabulary, string concatenatedCategories)
        {
            var pageKeywords = page.Regions.Keywords as ControlledKeywordsField;
            pageKeywords.Vocabulary.Value = concatenatedVocabulary;

            //update categories Oct 30, 2020
            var pageCategories = page.Regions.Categories as ControlledCategoriesField;
            pageCategories.Vocabulary.Value = concatenatedCategories;


            var controlledVocabBlocks = page.Blocks
                .Where(b => typeof(ControlledVocabularySearchBlock).IsAssignableFrom(b.GetType()))
                .ToList();
            foreach (var block in controlledVocabBlocks)
            {
                (block as ControlledVocabularySearchBlock)
                    .VocabularySettings
                    .Vocabulary
                    .Value = concatenatedVocabulary;

                (block as ControlledVocabularySearchBlock)
                    .CategorySettings
                    .Vocabulary
                    .Value = concatenatedCategories;
            }

        }

        public void UpdateKeywordVocabularyAsync(DynamicPost post, string concatenatedVocabulary, string concatenatedCategories)
        {
            var controlledVocabBlocks = post.Blocks
                .Where(b => typeof(ControlledVocabularySearchBlock).IsAssignableFrom(base.GetType()))
                .ToList();
            foreach (var block in controlledVocabBlocks)
            {
                (block as ControlledVocabularySearchBlock)
                    .VocabularySettings
                    .Vocabulary
                    .Value = concatenatedVocabulary;

                (block as ControlledVocabularySearchBlock)
                    .CategorySettings
                    .Vocabulary
                    .Value = concatenatedCategories;
            }
        }


        public async Task UpdateKeywordVocabularyAsync(PageBase pageBase)
        {
            try
            {
                var site = await _api.Sites.GetContentByIdAsync<CatfishWebsite>(pageBase.SiteId).ConfigureAwait(false);
                if ((pageBase as DynamicPage) != null)
                {
                    // (pageBase as DynamicPage).Regions.Keywords.Vocabulary.Value = site.Keywords.Value;
                    if (site.Keywords.Value != null)
                    {
                        ((Catfish.Models.Fields.ControlledKeywordsField)(pageBase as DynamicPage).Regions.Keywords).Vocabulary = site.Keywords.Value;
                       // ((Catfish.Models.Fields.ControlledKeywordsField)(pageBase as DynamicPage).Regions.Keywords).AllowedKeywords = site.Keywords.Value.Split(",").Select(kw => new Keyword() { Label = kw }).ToList();
                    }

                    //update site categories -- Mr Oct 30 2020
                    if (site.Categories.Value != null ) { 
                        ((Catfish.Models.Fields.ControlledCategoriesField)(pageBase as DynamicPage).Regions.Categories).Vocabulary = site.Categories.Value;
                     // ((Catfish.Models.Fields.ControlledCategoriesField)(pageBase as DynamicPage).Regions.Categories).AllowedCategories = site.Categories.Value.Split(",").Select(kw => new Keyword() { Label = kw }).ToList();
                    }
                }
                    
                var keywordSearchBlocks = pageBase.Blocks
                    .Where(b => typeof(ControlledVocabularySearchBlock).IsAssignableFrom(b.GetType()))
                    .Select(b => b as ControlledVocabularySearchBlock)
                    .ToList();

                foreach (var block in keywordSearchBlocks)
                {
                    block.VocabularySettings.Vocabulary.Value = site.Keywords.Value;
                    block.CategorySettings.Vocabulary.Value = site.Categories.Value;
                }

            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
        }

        public async Task UpdateKeywordVocabularyAsync(PostBase postBase)
        {
            try
            {
                var blog = await _api.Pages.GetByIdAsync<PageBase>(postBase.BlogId).ConfigureAwait(false);
                var site = await _api.Sites.GetContentByIdAsync<CatfishWebsite>(blog.SiteId).ConfigureAwait(false);
                (postBase as DynamicPost).Regions.Keywords.Vocabulary.Value = site.Keywords.Value;
                (postBase as DynamicPost).Regions.Categories.Vocabulary.Value = site.Categories.Value;

                var keywordSearchBlocks = postBase.Blocks
                    .Where(b => typeof(ControlledVocabularySearchBlock).IsAssignableFrom(b.GetType()))
                    .Select(b => b as ControlledVocabularySearchBlock)
                    .ToList();

                foreach (var block in keywordSearchBlocks)
                {
                    block.VocabularySettings.Vocabulary.Value = site.Keywords.Value;
                    block.CategorySettings.Vocabulary.Value = site.Categories.Value;
                }

            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
        }

        public async Task<string> getDefaultSiteKeywordAsync()
        {
            try
            {
                var site = _api.Sites.GetDefaultAsync();

                var siteContent = await _api.Sites.GetContentByIdAsync<CatfishWebsite>(site.Result.Id).ConfigureAwait(false);


                return siteContent.Keywords.Value != null ? siteContent.Keywords.Value : null;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public async Task<string> getDefaultSiteCategoryAsync()
        {
            try
            {
                var site = _api.Sites.GetDefaultAsync();

                var siteContent = await _api.Sites.GetContentByIdAsync<CatfishWebsite>(site.Result.Id).ConfigureAwait(false);
                return siteContent.Categories.Value != null ? siteContent.Categories.Value : null;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public async Task InitSiteStructureAsync(Guid siteId, string siteTypeId)
        {
            try
            {
                if (siteTypeId == typeof(WorkflowPortal).Name)
                {
                    var site = await _api.Sites.GetByIdAsync(siteId).ConfigureAwait(false);

                    var siteContent = await _api.Sites.GetContentByIdAsync(siteId).ConfigureAwait(false);
                    var workflowPageSettings = siteContent.Regions.WorkflowPagesContent;
                    if (workflowPageSettings.CreateSubmissionEntryPage == true)
                    {
                        try
                        {
                            //Making sure the submission-entry page exists
                            Guid? submissionEntryPageId = GetSystemPageId(siteId, "SubmissionEntryPage", true,
                            string.IsNullOrEmpty(workflowPageSettings.SubmissionEntryPage) ? "Start a Submission" : workflowPageSettings.SubmissionEntryPage);

                            //Making sure this page has at least one block of SubmissionEntryList
                            var page = await _api.Pages.GetByIdAsync(submissionEntryPageId.Value).ConfigureAwait(false);
                            if (page.Blocks.Where(b => typeof(SubmissionEntryPointList).IsAssignableFrom(Type.GetType(b.Type))).Any() == false)
                            {
                                page.Blocks.Add(new SubmissionEntryPointList());
                                await _api.Pages.SaveAsync<DynamicPage>(page).ConfigureAwait(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            _errorLog.Log(new Error(ex));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
        }

        protected Guid? GetSystemPageId(Guid siteId, string pageKey, bool createIfNotExist, string pageTitleIfShouldCreate)
        {
            try
            {
                SystemPage pageInfo = _db.SystemPages
                    .Where(pg => pg.PageKey == "SubmissionEntryPage" && pg.SiteId == siteId)
                    .FirstOrDefault();

                if (createIfNotExist == false)
                {
                    if (pageInfo == null)
                        return null;
                    else
                    {
                        try
                        {
                            var t = _api.Pages.GetByIdAsync(pageInfo.PageId);
                            t.Wait();
                            return t.Result.Id;
                        }
                        catch (Exception ex)
                        {
                            _errorLog.Log(new Error(ex));
                            return null;
                        }

                    }
                }
                else
                {
                    if (pageInfo == null)
                        pageInfo = new SystemPage() { PageKey = pageKey, SiteId = siteId };
                    else
                    {
                        try
                        {
                            var t = _api.Pages.GetByIdAsync(pageInfo.PageId);
                            t.Wait();
                            var loadedPage = t.Result;

                            if (loadedPage != null)
                                return loadedPage.Id;
                        }
                        catch (Exception ex)
                        {
                            _errorLog.Log(new Error(ex));
                            return null;
                        }
                    }

                    //If the execution comes here, then we need to create a new page and update the page info entry
                    var task = _api.Pages.CreateAsync<StandardPage>();
                    task.Wait();

                    StandardPage newPage = task.Result;
                    newPage.SiteId = siteId;
                    newPage.Published = DateTime.Now;
                    newPage.Title = pageTitleIfShouldCreate;

                    Task savePageTask = _api.Pages.SaveAsync<StandardPage>(newPage);
                    savePageTask.Wait();

                    pageInfo.PageId = newPage.Id;
                    _db.SaveChanges();

                    return newPage.Id;
                }
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }
    }
}

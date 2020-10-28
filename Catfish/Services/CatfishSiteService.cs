using Catfish.Models;
using Catfish.Models.Blocks;
using Catfish.Models.Fields;
using Catfish.Models.SiteTypes;
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
        public CatfishSiteService(IApi api)
        {
            _api = api;
        }

        public async Task UpdateKeywordVocabularyAsync(SiteContentBase siteContentBase)
        {
            Guid siteId = siteContentBase.Id;
            var siteTypeId = siteContentBase.TypeId;

            if (siteTypeId == typeof(CatfishWebsite).Name || siteTypeId == typeof(WorkflowPortal).Name)
            {
                //Get the contents of the given site
                var siteContent = await _api.Sites.GetContentByIdAsync(siteId).ConfigureAwait(false);
                
                //Gets the keywords field of the site settings
                var keywordsField = siteContent.Regions.Keywords as Piranha.Extend.Fields.TextField;
                if (!string.IsNullOrWhiteSpace(keywordsField.Value))
                {
                    //Keywords
                    var keywords = keywordsField.Value.Split(new char[] { ',', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    string concatenatedKeywords = string.Join(",", keywords);

                    //Get all the pages of the site and update their keywords
                    var pages = await _api.Pages.GetAllAsync(siteId).ConfigureAwait(false);
                    foreach(var page in pages)
                    {
                        try
                        {
                            var pageKeywords = page.Regions.Keywords as ControlledKeywordsField;
                            pageKeywords.Vocabulary.Value = concatenatedKeywords;
                            UpdateKeywordVocabularyAsync(page, concatenatedKeywords);
                            await _api.Pages.SaveAsync<DynamicPage>(page).ConfigureAwait(false);
                        }
                        catch (Exception)
                        {
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
                            UpdateKeywordVocabularyAsync(post, concatenatedKeywords);
                            await _api.Posts.SaveAsync<DynamicPost>(post).ConfigureAwait(false);
                        }
                        catch (Exception)
                        {
                        }
                    }

                }
            }
        }

        protected void UpdateKeywordVocabularyAsync(DynamicPage page, string concatenatedVocabulary)
        {
            var controlledVocabBlocks = page.Blocks
                .Where(b => typeof(ControlledVocabularySearchBlock).IsAssignableFrom(b.GetType()))
                .ToList();
            foreach(var block in controlledVocabBlocks)
            {
                (block as ControlledVocabularySearchBlock)
                    .VocabularySettings
                    .Vocabulary
                    .Value = concatenatedVocabulary;
            }
        }

        protected void UpdateKeywordVocabularyAsync(DynamicPost post, string concatenatedVocabulary)
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
            }
        }


        public async Task UpdateKeywordVocabularyAsync(PageBase pageBase)
        {
            try
            {
                var site = await _api.Sites.GetContentByIdAsync<CatfishWebsite>(pageBase.SiteId).ConfigureAwait(false);
                (pageBase as DynamicPage).Regions.Keywords.Vocabulary.Value = site.Keywords.Value;

                var keywordSearchBlocks = pageBase.Blocks
                    .Where(b => typeof(ControlledVocabularySearchBlock).IsAssignableFrom(b.GetType()))
                    .Select(b => b as ControlledVocabularySearchBlock)
                    .ToList();

                foreach (var block in keywordSearchBlocks)
                    block.VocabularySettings.Vocabulary.Value = site.Keywords.Value;

            }
            catch (Exception)
            {

            }
        }

        public async Task UpdateKeywordVocabularyAsync(PostBase postBase)
        {
            try
            {
                var blog = await _api.Pages.GetByIdAsync<PageBase>(postBase.BlogId).ConfigureAwait(false);
                var site = await _api.Sites.GetContentByIdAsync<CatfishWebsite>(blog.SiteId).ConfigureAwait(false);
                (postBase as DynamicPost).Regions.Keywords.Vocabulary.Value = site.Keywords.Value;

                var keywordSearchBlocks = postBase.Blocks
                    .Where(b => typeof(ControlledVocabularySearchBlock).IsAssignableFrom(b.GetType()))
                    .Select(b => b as ControlledVocabularySearchBlock)
                    .ToList();

                foreach (var block in keywordSearchBlocks)
                    block.VocabularySettings.Vocabulary.Value = site.Keywords.Value;

            }
            catch (Exception)
            {

            }
        }



    }
}

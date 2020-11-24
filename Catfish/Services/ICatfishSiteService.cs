using Piranha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public interface ICatfishSiteService
    {
        public Task UpdateKeywordVocabularyAsync(SiteContentBase siteContentBase);
        public Task UpdateKeywordVocabularyAsync(PageBase pageBase);
        public Task UpdateKeywordVocabularyAsync(PostBase post);
        public Task<string> getDefaultSiteKeywordAsync();
        public Task<string> getDefaultSiteCategoryAsync();
        public void UpdateKeywordVocabularyAsync(DynamicPage page, string concatenatedVocabulary, string concatenatedCategories);
        public void UpdateKeywordVocabularyAsync(DynamicPost post, string concatenatedVocabulary, string concatenatedCategories);
        public Task InitSiteStructureAsync(Guid siteId, string siteTypeId);
    }
}

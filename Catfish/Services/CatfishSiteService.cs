using Catfish.Models.Fields;
using Catfish.Models.SiteTypes;
using Piranha;
using Piranha.Models;
using Piranha.Services;
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
        public async Task UpdatePageSettingsAsync(Guid siteId, string siteTypeId)
        {
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

                    //Get all the pages of the site
                    var pages = await _api.Pages.GetAllAsync(siteId).ConfigureAwait(false);
                    foreach(var page in pages)
                    {
                        try
                        {
                            var pageKeywords = page.Regions.Keywords as ControlledKeywordsField;
                            pageKeywords.Vocabulary.Value = string.Join(",", keywords);
                            await _api.Pages.SaveAsync<DynamicPage>(page).ConfigureAwait(false);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
    }
}

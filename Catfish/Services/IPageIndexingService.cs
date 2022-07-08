using Catfish.Core.Models.Solr;
using Piranha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public interface IPageIndexingService
    {
        public void IndexPage(PageBase page);
        public void IndexPost(PostBase post);
        Task<List<Site>> GetSitesList();
        bool IndexSite(Guid siteId, string siteType);

    }
}

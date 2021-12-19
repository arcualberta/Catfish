using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Core.Services.Solr;
using Catfish.Services;
using ElmahCore;
using Microsoft.AspNetCore.Mvc;
using Piranha.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolrController : ControllerBase
    {
        private readonly IQueryService QueryService;
        private readonly IPageIndexingService _pageIndexingService;
        private readonly ISolrBatchService _solrBatchService;
        public SolrController(IQueryService queryService, IPageIndexingService pageIndexingService, ISolrBatchService solrBatchService)
        {
            QueryService = queryService;
            _pageIndexingService = pageIndexingService;
            _solrBatchService = solrBatchService;
        }

        [Route("freetext")]
        public IList<SolrEntry> FreeText([FromForm] string searchTerm)
        {
            try
            {
                var parameters = new SearchParameters();
                parameters.FreeSearch = searchTerm;
                IList<SolrEntry> result = QueryService.FreeSearch(parameters);
                return result;
            }
            catch (Exception ex)
            {
                HttpContext.Request.HttpContext.RiseError(ex);
                return new List<SolrEntry>();
            }
        }

        [Route("keywords")]
        public IList<SolrEntry> Keywords([FromForm] string[] keywords, [FromForm] string[] categories)
        {
            try
            {
                //var parameters = new SearchParameters();
                //parameters.FreeSearch = searchTerm;
                //IList<SolrEntry> result = QueryService.FreeSearch(parameters);

                if (categories.Length == 0 || !categories.Where(c => !string.IsNullOrWhiteSpace(c)).Any())
                    categories = new string[] { "*" };

                IList<SolrEntry> result = QueryService.KeywordSearch(keywords, categories);
                return result;

                ////IList<SolrEntry> result = new List<SolrEntry>();

                ////for (int i = 0; i < 2 * keywords.Length; ++i)
                ////{
                ////    SolrEntry entry = new SolrEntry()
                ////    {
                ////        Permalink = "http://google.com",
                ////        Id = Guid.NewGuid()
                ////    };

                ////    entry.AddContent(Guid.NewGuid(), "Housed in the Department of Art and Design, the Research - Creation and Social Justice CoLABoratory(the CoLAB) has been championing and nurturing interdisciplinary and intersectional research - creation since 2014.The CoLAB brings together key researchers at the University of Alberta with national and international a");
                ////    entry.AddContent(Guid.NewGuid(), "3–91 Fine Arts Building, <br />University of Alberta");

                ////    entry.SetTitle(Guid.NewGuid(), "Hello World");

                ////    entry.Images.Add("https://icatcare.org/app/uploads/2018/07/Thinking-of-getting-a-cat.png");
                ////    result.Add(entry);
                ////}
                ////return result;
            }
            catch (Exception ex)
            {
                HttpContext.RiseError(ex);
                return new List<SolrEntry>();
            }
        }

        [Route("index")]
        public void IndexSite([FromForm] Guid siteId, [FromForm] string siteTypeId)
        {
            _pageIndexingService.IndexSite(siteId, siteTypeId);
            _solrBatchService.IndexItems(true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services.Solr;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IQueryService QueryService;
        public SearchController(IQueryService queryService)
        {
            QueryService = queryService;
        }

        [Route("freetext")]
        public IList<SolrEntry> FreeText([FromForm] string searchTerm)
        {
            var parameters = new SearchParameters();
            parameters.FreeSearch = searchTerm;
            IList<SolrEntry> result = QueryService.FreeSearch(parameters);
            return result;
        }

        [Route("keywords")]
        public IList<SolrEntry> Keywords([FromForm] string[] searchTerms, string category)
        {
            //var parameters = new SearchParameters();
            //parameters.FreeSearch = searchTerm;
            //IList<SolrEntry> result = QueryService.FreeSearch(parameters);

            IList<SolrEntry> result = new List<SolrEntry>();

            for (int i = 0; i < 10; ++i)
            {
                SolrEntry entry = new SolrEntry()
                {
                    Permalink = "http://google.com"
                };

                entry.AddContent(Guid.NewGuid(), "Housed in the Department of Art and Design, the Research - Creation and Social Justice CoLABoratory(the CoLAB) has been championing and nurturing interdisciplinary and intersectional research - creation since 2014.The CoLAB brings together key researchers at the University of Alberta with national and international a");
                entry.AddContent(Guid.NewGuid(), "3–91 Fine Arts Building, <br />University of Alberta");

                entry.Images.Add("https://icatcare.org/app/uploads/2018/07/Thinking-of-getting-a-cat.png");
                result.Add(entry);
            }
            return result;
        }

    }
}

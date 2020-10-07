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
        public IList<SolrEntry> FreeText(string searchTerm)
        {
            var parameters = new SearchParameters();
          
            

            parameters.FreeSearch = searchTerm;
            //parameters.FreeSearch = "aldflajdsfjasd";
            parameters.FreeSearch = "ponyo";



            IList<SolrEntry> result = QueryService.FreeSearch(parameters);
            return result;
        }
    }
}

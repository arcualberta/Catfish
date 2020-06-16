using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Services.Solr;
using Catfish.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Catfish.Core.Services.Solr;
using Catfish.Core.Models.Solr;
using SolrNet;

namespace Catfish.Pages
{
    public class SearchModel : PageModel
    {
        public string SearchTerm { get; set; }
        //public string SolrUrl { get; set; }

        public SolrQueryResults<SolrItemModel> Results { get; set; }

        private readonly IQueryService QueryService;
        public SearchModel(IQueryService queryService)
        {
           // SolrUrl = config.GetSolrUrl();
            QueryService = queryService;
        }

        public void OnGet()
        {
            Results = new SolrQueryResults<SolrItemModel>();
        }

        public void OnPost()
        {
            Results = new SolrQueryResults<SolrItemModel>(); //TODO: Run the query and get this list.
            var parameters = new SearchParameters();
            parameters.FreeSearch = "schema";//SearchTerm;
            Results = QueryService.Results(parameters);
        }

    }
}
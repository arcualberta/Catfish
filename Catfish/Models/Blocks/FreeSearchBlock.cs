using Catfish.Core.Models;
using Catfish.Services;
using Piranha.Extend;
using Piranha.Extend.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Catfish.Core.Services.Solr;
using Catfish.Core.Models.Solr;
using SolrNet;

namespace Catfish.Models.Blocks
{
    [BlockType(Name = "Free Search", Category = "Search", Component = "free-search", Icon = "fas fa-th-list")]
    public class FreeSearchBlock : Block
    {
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        //public string SolrUrl { get; set; }

        public IList<SolrEntry> Results { get; set; }

        

        private readonly IQueryService QueryService;
        public FreeSearchBlock(IQueryService queryService)
        {
            // SolrUrl = config.GetSolrUrl();
            QueryService = queryService;
            CssVal = new TextField();
        }


        public TextField CssVal { get; set; }
        public string GetCss()
        {
            if (CssVal != null)
            {
                return CssVal.Value;
            }

            return "";
        }

        //public void OnGet()
        //{
        //    Results = new List<SolrEntry>();
        //}

        public void OnPost()
        {
            //Results = new SolrQueryResults<SolrItemModel>(); //TODO: Run the query and get this list.
            var parameters = new SearchParameters();
            parameters.FreeSearch = SearchTerm;
            //Results = QueryService.GetEntities(parameters);
            Results = QueryService.FreeSearch(parameters);
        }
    }

}

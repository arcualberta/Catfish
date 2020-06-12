using Catfish.Core.Models.Solr;
using Catfish.Core.Services.Solr;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Catfish.Core.Controllers.Solr
{
    public class SolrController //: Controller
    {
        private IQueryService _srv;
        public SolrController(QueryService srv)
        {
            _srv = srv;
        }

        public IActionResult Index()
        {
            return null;// View();
        }

        public ActionResult Search(SearchParameters parameters)
        {


            return null;// View(_srv.Search(parameters));
        }
    }
}

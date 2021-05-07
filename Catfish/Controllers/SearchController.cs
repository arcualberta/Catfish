using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Core.Services.Solr;
using Catfish.Models.ViewModels;
using ElmahCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Catfish.Controllers
{
    public class SearchController : Controller
    {
        private readonly IQueryService _queryService;
        private readonly ISolrService _solr;
        private readonly ErrorLog _errorLog;

        public SearchController(IQueryService srv, ISolrService solr, ErrorLog errorLog)
        {
            _queryService = srv;
            _solr = solr;
            _errorLog = errorLog;
        }
        // GET: SearchController
        public ActionResult Index(string searchTerm)
        {
            var parameters = new SearchParameters();
            parameters.FreeSearch = searchTerm;
            var results = _queryService.FreeSearch(parameters);
            return View("Results", results);
        }

        // GET: SearchController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SearchController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SearchController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SearchController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SearchController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SearchController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SearchController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        // [ValidateAntiForgeryToken]
        public SearchResult AdvanceSearch([FromForm] List<SearchFieldConstraint> constraints, string simpleSearchTerm, string searchMode, int start = 0, int itemPerPage = 25)
        {
            try
            {
                string jsonConst = HttpContext.Request.Query["constraints"][0];
                var _contraint = JsonConvert.DeserializeObject<List<SearchFieldConstraint>>(jsonConst);

                if (searchMode == "simple")
                    return _solr.Search(simpleSearchTerm, start, itemPerPage);

                if (_contraint.Count > 0)
                    return _solr.Search(_contraint.ToArray(), start, itemPerPage);
                else
                    return _solr.Search(null as string, start, itemPerPage);

            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                throw new Exception("An internal error occurred");
            }
        }
    }
}

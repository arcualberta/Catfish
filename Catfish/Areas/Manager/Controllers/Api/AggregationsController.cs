using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Catfish.Models.ViewModels;
using Newtonsoft.Json;

namespace Catfish.Controllers.Api
{

    public class AggregationRelationParameters
    {
        public string Guid { get; set; } = "";
        public int Page { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 10;
        public string Query { get; set; } = "*";
        public string SortBy { get; set; } = "";
    }

    public class AggregationsController : CatfishController
    {
        // GET: apix/Aggregation
        public ContentResult Index([System.Web.Http.FromUri] AggregationRelationParameters parameters)
        {
            SecurityService.CreateAccessContext();

            CFAggregationAssociationsViewModel test = new CFAggregationAssociationsViewModel
            {
                Page = parameters.Page,
                TotalItems = 1,
                ItemsPerPage = 1,
                TotalPages = 1
            };


            int total;
            test.Data = AggregationService.Index(
                out total, 
                parameters.Query, 
                test.Page, 
                parameters.ItemsPerPage).Select(x => new CFAggregationIndexViewModel(x));

            test.TotalItems = total;
            test.ItemsPerPage = parameters.ItemsPerPage;
            test.TotalPages = (int)Math.Ceiling((double)test.TotalItems / test.ItemsPerPage);

            var list = JsonConvert.SerializeObject(test,
                Formatting.Indented
            );

            return Content(list, "application/json");            
        }

        [HttpGet]
        public JsonResult GetParents(AggregationRelationParameters parameters) {

            //int start = page * itemsPerPage;
            //string sortField = SortField(sortAttributeMappingId);
            //return Db.Items.FromSolr(query, out total, entityTypeFilter, start, itemsPerPage, sortField, sortAsc);

            int start = parameters.Page * parameters.ItemsPerPage;
            int total;
            //Db.Items.FromSolr

            List<CFAggregation> aggregations = AggregationService.Index(
                out total, 
                parameters.Query, 
                parameters.Page, 
                parameters.ItemsPerPage).ToList();
            

            return Json(new {
                total,
                aggregations
            });
            
            
        }

        [HttpGet]
        public JsonResult GetChildren() { return null; }

        [HttpGet]
        public JsonResult GetRelations() { return null; }

        [HttpPost]
        public JsonResult PostParents() { return null; }

        [HttpPost]
        public JsonResult PostChildren() { return null; }

        [HttpPost]
        public JsonResult PostRelations() { return null; }
    }
}
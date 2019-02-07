using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Catfish.Areas.Manager.Models.ViewModels;
using Catfish.Core.Models;
using Newtonsoft.Json;

namespace Catfish.Controllers.Api
{

    public class AggregationRelationParameters
    {
        public string guid = "";
        public int page = 1;
        public int itemsPerPage = 1;
        public string query = "";
    }

    public class AggregationsController : CatfishController
    {
        // GET: apix/Aggregation
        public JsonResult Index()
        {
            SecurityService.CreateAccessContext();

            // llama a la base de datos para obtener las cosas paginadas

            CFAggregationAssociationsViewModel test = new CFAggregationAssociationsViewModel
            {
                CurrentPage = 1,
                TotalPages = 1
            };



            List<CFAggregationAssociationsViewModel> testList = new List<CFAggregationAssociationsViewModel>
            {
                test
            };
            int total;
            test.Data = AggregationService.Index(out total, "", test.CurrentPage, 10);

            test.TotalPages = total;

            return Json(test, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetParents(AggregationRelationParameters parameters) {

            //int start = page * itemsPerPage;
            //string sortField = SortField(sortAttributeMappingId);
            //return Db.Items.FromSolr(query, out total, entityTypeFilter, start, itemsPerPage, sortField, sortAsc);

            int start = parameters.page * parameters.itemsPerPage;
            int total;
            //Db.Items.FromSolr

            List<CFAggregation> aggregations = AggregationService.Index(
                out total, 
                parameters.query, 
                parameters.page, 
                parameters.itemsPerPage).ToList();
            

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
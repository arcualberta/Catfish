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
        public int id { get; set; } = 0;
        public int Page { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 10;
        public string Query { get; set; } = "*:*";
        public string SortBy { get; set; } = "";
    }

    public class AggregationsController : CatfishController
    {
        // GET: apix/Aggregation
        [HttpGet]
        public ContentResult Index([System.Web.Http.FromUri] AggregationRelationParameters parameters)
        {
            SecurityService.CreateAccessContext();

            CFAggregationAssociationsViewModel response = new CFAggregationAssociationsViewModel
            {
                Page = parameters.Page,
                TotalItems = 1,
                ItemsPerPage = 1,
                TotalPages = 1
            };


            int total;
            response.Data = AggregationService.Index(
                out total, 
                parameters.Query, 
                response.Page, 
                parameters.ItemsPerPage).Select(x => new CFAggregationIndexViewModel(x));

            response.TotalItems = total;
            response.ItemsPerPage = parameters.ItemsPerPage;
            response.TotalPages = (int)Math.Ceiling((double)response.TotalItems / response.ItemsPerPage);

            string responseString = JsonConvert.SerializeObject(response,
                Formatting.None
            );

            return Content(responseString, "application/json");            
        }

        [HttpGet]
        public ContentResult GetParents(AggregationRelationParameters parameters) {
            // XXX Fixme, change from getting children to get parents
            SecurityService.CreateAccessContext();

            CFAggregationAssociationsViewModel response = new CFAggregationAssociationsViewModel
            {
                Page = parameters.Page,
                TotalItems = 1,
                ItemsPerPage = 1,
                TotalPages = 1
            };


            int total;

            //string mappedGuid =
            CFAggregation test = AggregationService.GetAggregation(parameters.id);
            
            response.Data = AggregationService.Parents(
                test.MappedGuid,
                out total,
                parameters.Query,
                response.Page,
                parameters.ItemsPerPage).Select(x => new CFAggregationIndexViewModel(x));

            response.TotalItems = total;
            response.ItemsPerPage = parameters.ItemsPerPage;
            response.TotalPages = (int)Math.Ceiling((double)response.TotalItems / response.ItemsPerPage);

            string responseString = JsonConvert.SerializeObject(response,
                Formatting.None
            );

            return Content(responseString, "application/json");

        }

        [HttpGet]
        public ContentResult GetChildren([System.Web.Http.FromUri] AggregationRelationParameters parameters) {

            SecurityService.CreateAccessContext();

            CFAggregationAssociationsViewModel response = new CFAggregationAssociationsViewModel
            {
                Page = parameters.Page,
                TotalItems = 1,
                ItemsPerPage = 1,
                TotalPages = 1
            };


            // XXX Fix me
            int total;

            //string mappedGuid =
            CFAggregation test = AggregationService.GetAggregation(parameters.id);

            response.Data = AggregationService.Children(
                test.MappedGuid,
                out total,
                parameters.Query,
                response.Page,
                parameters.ItemsPerPage).Select(x => new CFAggregationIndexViewModel(x));

            response.TotalItems = total;
            response.ItemsPerPage = parameters.ItemsPerPage;
            response.TotalPages = (int)Math.Ceiling((double)response.TotalItems / response.ItemsPerPage);

            string responseString = JsonConvert.SerializeObject(response,
                Formatting.None
            );

            return Content(responseString, "application/json");

        }

        [HttpGet]
        public JsonResult GetRelations() { return null; }

        [HttpPost]
        public JsonResult AddParents(int id, int[] objectIds) {
            SecurityService.CreateAccessContext();
            CFAggregation aggregation = AggregationService.GetAggregation(id);

            foreach(CFAggregation parent in AggregationService.GetAggregations(objectIds))
            {                
                parent.AddChild(aggregation);
                Db.Entry(parent).State = System.Data.Entity.EntityState.Modified;

            }

            Db.SaveChanges(User.Identity);
            return Json("Ok");

        }

        [HttpPost]
        public JsonResult RemoveParents(int id, int[] objectIds)
        {
            SecurityService.CreateAccessContext();
            CFAggregation aggregation = AggregationService.GetAggregation(id);

            foreach (CFAggregation parent in AggregationService.GetAggregations(objectIds))
            {                
                parent.RemoveChild(aggregation);
            }

            Db.SaveChanges(User.Identity);
            return Json("");
        }

        [HttpPost]
        public JsonResult AddChildren(int id, int[] objectIds) {
            SecurityService.CreateAccessContext();
            CFAggregation aggregation = AggregationService.GetAggregation(id);

            foreach(CFAggregation child in AggregationService.GetAggregations(objectIds))
            {
                //CFAggregation child = AggregationService.GetAggregation(childId);
                aggregation.AddChild(child);
                Db.Entry(child).State = System.Data.Entity.EntityState.Modified;

            }
            Db.Entry(aggregation).State = System.Data.Entity.EntityState.Modified;

            Db.SaveChanges(User.Identity);
            return Json("");
        }

        [HttpPost]
        public JsonResult RemoveChildren(int id, int[] objectIds)
        {
            SecurityService.CreateAccessContext();
            CFAggregation aggregation = AggregationService.GetAggregation(id);

            foreach (CFAggregation child in AggregationService.GetAggregations(objectIds))
            {
                aggregation.RemoveChild(child);
            }

            Db.SaveChanges(User.Identity);
            return Json("");
        }

        [HttpPost]
        public JsonResult PostRelations() { return null; }
    }
}
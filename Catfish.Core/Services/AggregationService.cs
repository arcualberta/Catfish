﻿using Catfish.Core.Contexts;
using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public class AggregationService : EntityService
    {
        /// <summary>
        /// Create an instance of the AggregationService.
        /// </summary>
        /// <param name="db">The database context containing the needed Items.</param>
        /// 
        public AggregationService(CatfishDbContext db) : base(db) { }

        private bool DefaultIsAdminFalse(string none) { return false; }

        /// <summary>
        /// Get an aggregation from the database.
        /// </summary>
        /// <param name="id">The id of the Aggregation to obtain.</param>
        /// <returns>The requested Aggregation from the database. A null value is returned if no item is found.</returns>
        public CFAggregation GetAggregation(int id, AccessMode accessMode = AccessMode.Read)
        {
            //SecurityService
            return Db.Aggregations.FindAccessible(AccessContext.current.IsAdmin,
                AccessContext.current.AllGuids,
                accessMode)
                .Where(i => i.Id == id).FirstOrDefault();
        }

        public List<CFAggregation> GetAggregations(IEnumerable<int> ids, AccessMode accessMode = AccessMode.Read)
        {
            List<CFAggregation> result = new List<CFAggregation>();

            foreach(int id in ids)
            {
                result.Add(GetAggregation(id, accessMode));
            }

            return result;
        }

        ///// <summary>
        ///// Get an aggregation from the database.
        ///// </summary>
        ///// <param name="mappedId">The mappedId of the Aggregation to obtain.</param>
        ///// <returns>The requested Aggregation from the database. A null value is returned if no item is found.</returns>
        //public CFAggregation GetAggregation(string mappedId, AccessMode accessMode = AccessMode.Read)
        //{
        //    //SecurityService
        //    return Db.Aggregations.FindAccessible(AccessContext.current.IsAdmin,
        //        AccessContext.current.AllGuids,
        //        accessMode)
        //        .Where(i => i.MappedGuid == mappedId).FirstOrDefault();
        //}

        public IEnumerable<CFAggregation> Index(
            out int total,
            string query = "*:*",            
            int page = 1,
            int itemsPerPage = 10            
            ) 
        {

            int start = (page-1) * itemsPerPage;

            List<CFAggregation> data =  Db.Aggregations.FromSolr(query, 
                out total, 
                "", 
                start, 
                itemsPerPage, 
                null, 
                false).ToList();

            return data;            
        }

        public IEnumerable<CFAggregation> Parents(
            string id,
            out int total,
            string query = "*:*",
            int page = 1,
            int itemsPerPage = 10)
        {

            // get parents ids y modifica query
            //related_ss: ("da1faf51-aec8-4c37-9fe6-1e72e22f1340" "dc2c4b3b-51e2-43dd-8ac7-97e82d01f7a4")
            // get from id

            //int oneTotal;
            //CFAggregation test = Db.Aggregations.FromSolr("id:" + id, out oneTotal, "", 1, 1, null, false).FirstOrDefault();            

            CFAggregation aggregation = Db.Aggregations.Where(x => id == x.MappedGuid).FirstOrDefault();

            string parentIds = String.Join(" ", aggregation.ParentMembers);
            aggregation.ParentMembers.ToList().ToString();

            //related_ss: ("da1faf51-aec8-4c37-9fe6-1e72e22f1340" "dc2c4b3b-51e2-43dd-8ac7-97e82d01f7a4") AND id:6c87d53e-e897-40f7-86dc-8f21185d83ef

            string newQuery = query;

            if (newQuery.Length > 0)
            {
                newQuery += " AND ";
            }

            newQuery += "id:(" + parentIds + ")";

            return Index(out total, newQuery, page, itemsPerPage);

        }

        public IEnumerable<CFAggregation> Children(
            string id,
            out int total,
            string query = "*:*",
            int page = 1,
            int itemsPerPage = 10)            
        {
            string newQuery = query;

            if (newQuery.Length > 0)
            {
                newQuery += " AND ";
            }

            newQuery += " parents_ss: " + id;

            return Index(out total, newQuery, page, itemsPerPage);
        }

        public IEnumerable<CFAggregation> Related(
            string id,
            out int total,
            string query = "*:*",
            int page = 1,
            int itemsPerPage = 10)
        {
            string newQuery = query;

            if (newQuery.Length > 0)
            {
                newQuery += " AND ";
            }

            newQuery += " related_ss: " + id;

            return Index(out total, newQuery, page, itemsPerPage);
        }

    }
}

using Catfish.Core.Contexts;
using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

        private int ReIndexBucket(IEnumerable<int> bucket)
        {
            CatfishDbContext db = new CatfishDbContext();
            
            List<Dictionary<string, object>> aggrigations = new List<Dictionary<string, object>>();

            foreach(int aggregationId in bucket)
            {
                CFAggregation aggregation = db.Aggregations.Find(aggregationId);
                aggrigations.Add(aggregation.ToSolrDictionary());
            }

            SolrService.SolrOperations.AddRange(aggrigations);
            SolrService.SolrOperations.Commit();

            return aggrigations.Count;
        }

        public class ReIndexStruct
        {
            public int ProcessedCount;
            public int ReadCount;
            public int BucketSize;
            public int TaskCount;
        }

        private static object ReIndexLock = new object();
        private static ReIndexStruct mReIndexState = null;
        public static ReIndexStruct ReIndexState
        {
            get {
                if (mReIndexState == null) {
                    mReIndexState = new ReIndexStruct()
                    {
                        ProcessedCount = 0,
                        ReadCount = 0,
                        BucketSize = 0,
                        TaskCount = 0
                    };
                }

                return mReIndexState;
            }
        }

        /// <summary>
        /// Reindexes the Solr documents.
        /// </summary>
        /// <param name="bucketSize">How many to reindex at any one time.</param>
        /// <param name="poolSize">The amount of indexing operations that can occur at a single time.</param>
        /// <returns></returns>
        public int ReIndex(int bucketSize)
        {
            object resultLock = new object();
            int result = 0;

            lock (ReIndexLock)
            {
                ReIndexState.ProcessedCount = 0;
                ReIndexState.ReadCount = 0;
                ReIndexState.BucketSize = bucketSize;
                ReIndexState.TaskCount = 0;

                IEnumerator<int> aggregations = Db.Aggregations.AsNoTracking()
                    .Select(a => a.Id).ToList().GetEnumerator();
                HttpContext currentContext = HttpContext.Current;
                bool continueLoop = true;

                List<Task> tasks = new List<Task>();

                while (continueLoop)
                {
                    // Get the sublist.
                    // This method was done to to efficency issues with EntityFrameworks Skip and Take method.
                    IList<int> aggregationSublist = new List<int>(bucketSize);
                    while (aggregationSublist.Count < bucketSize && aggregations.MoveNext())
                    {
                        aggregationSublist.Add(aggregations.Current);
                        ++ReIndexState.ReadCount;
                    }

                    continueLoop = aggregationSublist.Count >= bucketSize;

                    // Perform the indexing on the sublist
                    tasks.Add(Task.Factory.StartNew(new Action<object>((aggregationIds) =>
                        {
                            if (aggregationSublist != null && aggregationSublist.Count() > 0)
                            {
                                HttpContext.Current = currentContext; // Done to find the current user later on
                                int subResult = ReIndexBucket((IEnumerable<int>)aggregationIds);

                                lock (resultLock)
                                {
                                    result += subResult;
                                    ReIndexState.ProcessedCount = result;
                                    --ReIndexState.TaskCount;
                                }
                            }
                        }), aggregationSublist));

                    lock (resultLock)
                    {
                        ++ReIndexState.TaskCount;
                    }
                }

                Task.WaitAll(tasks.ToArray()); // Wait for all tasks to complete before continuing.
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

            // The query has been modified to only allow for aggrigations with an entity type to be shown.
            string modifiedQuery = string.Format("entitytype_s:* AND ({0})", query);

            List<CFAggregation> data =  new CatfishDbContext().Aggregations.FromSolr(modifiedQuery, 
                out total, 
                "", 
                start, 
                itemsPerPage, 
                null, 
                false).ToList();

            return data;            
        }

        // XXX Parents and Related can be refactored

        public IEnumerable<CFAggregation> Parents(
            string id,
            out int total,
            string query = "*:*",
            int page = 1,
            int itemsPerPage = 10)
        {


            CFAggregation aggregation = Db.Aggregations.Where(x => id == x.MappedGuid).FirstOrDefault();


            List<string> mappedGuids = aggregation.ParentMembers.Select(x => x.MappedGuid).ToList();
            string newQuery = query;
                     
            if (mappedGuids.Count > 0)
            {
                string parentIds = String.Join(" ", mappedGuids);
                if (newQuery.Length > 0)
                {
                    newQuery += " AND ";
                }
                newQuery += "id:(" + parentIds + ")";
            } else
            {
                newQuery = "";
            }                                

            return Index(out total, newQuery, page, itemsPerPage);

        }

        public IEnumerable<CFAggregation> Related(
            string id,
            out int total,
            string query = "*:*",
            int page = 1,
            int itemsPerPage = 10)
        {

            CFAggregation aggregation = Db.Aggregations.Where(x => id == x.MappedGuid).FirstOrDefault();
            List<string> mappedGuids = aggregation.RelatedMembers.Select(x => x.MappedGuid).ToList();

            string newQuery = query;

            if (mappedGuids.Count > 0)
            {
                string relatedIds = String.Join(" ", mappedGuids);
                if (newQuery.Length > 0)
                {
                    newQuery += " AND ";
                }
                newQuery += "id:(" + relatedIds + ")";
            }
            else
            {
                newQuery = "";
            }

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

        

    }
}

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

        private int ReIndexBucket(IEnumerable<int> bucket, string taskId)
        {
            CatfishDbContext db = new CatfishDbContext();
            var SolrOperations = CommonServiceLocator.ServiceLocator.Current.GetInstance<SolrNet.ISolrOperations<Dictionary<string, object>>>();

            int result = 0;

            foreach(int aggregationId in bucket)
            {
                CFAggregation aggregation = db.Aggregations.Find(aggregationId);

                if (aggregation.MappedGuid != aggregation.Guid)
                {
                    aggregation.MappedGuid = aggregation.Guid;
                    db.Entry(aggregation).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    // No need to update the entire model.
                    SolrOperations.Add(aggregation.ToSolrDictionary());
                }

                ++result;

                try
                {
                    ReIndexState.TaskProcessedCount[taskId] = result;
                }catch(KeyNotFoundException ex)
                {
                    ReIndexState.TaskProcessedCount.Add(taskId, result);
                }
            }
            
            SolrOperations.Commit();

            return result;
        }

        /// <summary>
        /// Reindexes the Solr documents.
        /// </summary>
        /// <param name="bucketSize">How many to reindex at any one time.</param>
        /// <param name="poolSize">The amount of indexing operations that can occur at a single time.</param>
        /// <returns></returns>
        public void ReIndex(int bucketSize)
        {
            if(System.Threading.Monitor.TryEnter(ReIndexState.ReIndexLock, 10))
            {
                try
                {
                    ReIndexState.ProcessedCount = 0;
                    ReIndexState.ReadCount = 0;
                    ReIndexState.BucketSize = bucketSize;
                    ReIndexState.TaskCount = 0;
                    ReIndexState.TaskProcessedCount.Clear();

                    int[] aggregations = Db.Aggregations.AsNoTracking()
                        .Select(a => a.Id).ToArray();

                    HttpContext currentContext = HttpContext.Current;

                    List<Task> tasks = new List<Task>();
                    int aggregationIndex = 0;

                    while (aggregationIndex < aggregations.Length)
                    {
                        // Get the sublist.
                        int aggregationSize = Math.Min(aggregations.Length - aggregationIndex, bucketSize);
                        int[] aggregationSublist = new int[aggregationSize];
                        Array.Copy(aggregations, aggregationIndex, aggregationSublist, 0, aggregationSize);

                        aggregationIndex += bucketSize;

                        // Perform the indexing on the sublist
                        tasks.Add(Task.Factory.StartNew(new Action<object>((aggregationIds) =>
                            {
                                string taskId = Task.CurrentId.Value.ToString();
                                lock (ReIndexState.ReIndexLock)
                                {
                                    ReIndexState.TaskProcessedCount.Add(taskId, 0);
                                }

                                if (aggregationSublist != null && aggregationSublist.Count() > 0)
                                {
                                    HttpContext.Current = currentContext; // Done to find the current user later on
                                int subResult = ReIndexBucket((IEnumerable<int>)aggregationIds, taskId);

                                    lock (ReIndexState.ReIndexLock)
                                    {
                                        ReIndexState.ProcessedCount += subResult;
                                        --ReIndexState.TaskCount;

                                        ReIndexState.TaskProcessedCount.Remove(taskId);
                                    }
                                }
                            }), aggregationSublist, TaskCreationOptions.PreferFairness | TaskCreationOptions.LongRunning));

                        ++ReIndexState.TaskCount;
                    }
                }
                finally
                {
                    System.Threading.Monitor.Exit(ReIndexState.ReIndexLock);
                }
            }
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
    
    public static class ReIndexState
    {
        public struct ReIndexStruct
        {
            public int ProcessedCount;
            public int ReadCount;
            public int BucketSize;
            public int TaskCount;

            public IDictionary<string, int> TaskProcessedCount;
        }

        public static object ReIndexLock = new object();
        
        public static int ProcessedCount = 0;
        public static int ReadCount = 0;
        public static int BucketSize = 0;
        public static int TaskCount = 0;

        public static IDictionary<string, int> TaskProcessedCount = new Dictionary<string, int>();

        public static bool IsIndexing {
            get
            {
                return TaskProcessedCount.Count > 0;
            }
        }

        public static ReIndexStruct GetAsStruct()
        {
            return new ReIndexStruct()
            {
                ProcessedCount = ProcessedCount,
                ReadCount = ReadCount,
                BucketSize = BucketSize,
                TaskCount = TaskCount,
                TaskProcessedCount = TaskProcessedCount
            };
        }
    }
}

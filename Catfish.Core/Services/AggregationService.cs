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

    }
}

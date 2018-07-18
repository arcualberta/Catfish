using Catfish.Core.Models;
using Catfish.Core.Services;
using CommonServiceLocator;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Helpers
{
    public static class XmlLinqExtensions
    {
        public static IQueryable<TSource> FromSolr<TSource>(this DbSet<TSource> set, string q, int start = 0, int rows = 1000000, string sortRowId = null, bool sortAscending = false) where TSource : CFXmlModel
        {
            if (SolrService.IsInitialized)
            {
                var options = new QueryOptions()
                {
                    StartOrCursor = new StartOrCursor.Start(start),
                    Rows = rows
                };

                if (!string.IsNullOrEmpty(sortRowId))
                {
                    options.OrderBy = new List<SortOrder>()
                    {
                        new SortOrder(sortRowId, sortAscending ? Order.ASC : Order.DESC)
                    }
                }

                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrIndex>>();
                var results = solr.Query(q, options).Select(s => s.Id).Distinct();

                return set.Where(p => results.Contains(p.Id));
            }

            throw new InvalidOperationException("The SolrService has not been initialized.");
        }
    }
}

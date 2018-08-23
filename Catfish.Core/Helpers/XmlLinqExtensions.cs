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
        public static IQueryable<TSource> FromSolr<TSource>(this DbSet<TSource> set, string q, out int total, int start = 0, int rows = int.MaxValue, string sortRowId = null, bool sortAscending = false) where TSource : CFXmlModel
        {
            if(start < 0)
            {
                start = 0;
            }

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
                    };
                }

                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrIndex>>();
                var results = solr.Query(q, options);
                total = results.NumFound;

                int resultsCount = results.Count();
                string query;

                if(resultsCount > 0)
                {
                    StringBuilder values = new StringBuilder(resultsCount << 3); // This method is twice as fast as String.Join
                    values.Append(results.First().Id.ToString());
                    for (int i = 1; i < resultsCount; ++i)
                    {
                        values.Append(',');
                        values.Append(results.ElementAt(i).Id);
                    }

                    query = string.Format("SELECT * FROM [dbo].[CFXmlModels] WHERE Id IN ({0})", values.ToString());
                }
                else
                {
                    query = "SELECT * FROM [dbo].[CFXmlModels] WHERE Id < 0"; // Produces an empty result
                }
                return set.SqlQuery(query).AsQueryable();

                //return set.Where(p => results.Contains(p.Id)); // the Contians method is slow because it creates several or expressions. 
                // More info can be found here: https://stackoverflow.com/questions/8107439/why-is-contains-slow-most-efficient-way-to-get-multiple-entities-by-primary-ke
            }

            throw new InvalidOperationException("The SolrService has not been initialized.");
        }
    }
}

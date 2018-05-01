using Catfish.Core.Models;
using Catfish.Core.Services;
using CommonServiceLocator;
using SolrNet;
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
        public static IQueryable<TSource> FromSolr<TSource>(this DbSet<TSource> set, string q) where TSource : CFXmlModel
        {
            if (SolrService.IsInitialized)
            {
                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrIndex>>();
                var results = solr.Query(q).Select(s => s.Id).Distinct();

                return set.Where(p => results.Contains(p.Id));
            }

            throw new InvalidOperationException("The SolrService has not been initialized.");
        }
    }
}

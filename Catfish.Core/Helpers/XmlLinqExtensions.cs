using Catfish.Core.Models;
using Catfish.Core.Services;
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
        public static IQueryable<TSource> FromSolr<TSource>(this DbSet<TSource> set, string q) where TSource : XmlModel
        {
            if (SolrService.IsInitialized)
            {
                
                //var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrIndex>>();
            }
            else
            {
                throw new InvalidOperationException("SolrService has not been initialized correctly. Please make sure that you have defined the app parameter SolrServer in your web.config.");
            }

            return null;
        }
    }
}

using Catfish.Core.Models;
using Catfish.Core.Models.Access;
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

        public static IQueryable<TSource> FindAccessible<TSource>(this DbSet<TSource> set, ICollection<Guid> Guids, AccessMode mode) where TSource : CFXmlModel
        {
            //select distinct id, content
            //string guidList = string.Join(",", Guids);
            string guidList = string.Join(",", Array.ConvertAll(Guids.ToArray(), g => "'" + g + "'"));
            string sqlQuery = $@"
            SELECT *
            FROM
            (SELECT 
                CFXmlModels.*,
                CAST( [content] AS NVARCHAR(MAX) ) AS [contentCast],
                pref.value('access-definition[1]/access-modes[1]', 'int') AS Mode,
                pref.value('access-guid[1]', 'char') AS Guid
                FROM CFXmlModels CROSS APPLY
                content.nodes('//access/access-group') AS contentCast(pref)
            ) AS Result
            WHERE Mode & {(int)mode} = {(int)mode} AND Guid IN ({(string)guidList})
            ";

            return set.SqlQuery(sqlQuery).AsQueryable().Distinct();
        }
    }

   
}

using Catfish.Core.Contexts;
using Catfish.Core.Models;
using Catfish.Core.Models.Access;
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


        //XXX Asses query parameter order
        public static IQueryable<TSource> FromSolr<TSource>(
        this DbSet<TSource> set,        
        string solrQuery,        
        out int total,
        string entityTypeFilter = "",
        int start = 0,
        int rows = int.MaxValue,
        string sortRowId = null,
        bool sortAscending = false
        ) where TSource : CFXmlModel
        {
            if (start < 0)
            {
                start = 0;
            }

            bool isAdmin = AccessContext.current.IsAdmin;
            ICollection<Guid> guids = AccessContext.current.AllGuids;

            if (SolrService.IsInitialized)
            {
                var options = new QueryOptions()
                {
                    StartOrCursor = new StartOrCursor.Start(start),
                    Rows = rows
                };

                LinkedList<SortOrder> orderBy = new LinkedList<SortOrder>();
                {
                    new SortOrder("created_dt", Order.ASC);
                };

                //orderBy.AddLast(new SortOrder("created_date", Order.ASC));

                if (!string.IsNullOrEmpty(sortRowId))
                {
                    orderBy.AddFirst(new SortOrder(sortRowId, sortAscending ? Order.ASC : Order.DESC));
                }

                options.OrderBy = orderBy;

                //if (!string.IsNullOrEmpty(sortRowId))
                //{
                //    options.OrderBy = new[]
                //    {
                //        new SortOrder(sortRowId, sortAscending ? Order.ASC : Order.DESC),
                //        //new SortOrder("updated_date", Order.DESC)
                //        //new SortOrder(sortRowId, sortAscending ? Order.ASC : Order.DESC)
                //    };
                //}

                //score desc, updated_date desc

                var solr = ServiceLocator.Current.GetInstance<ISolrOperations<SolrIndex>>();

                // Create filter query

                // for solr based listing we just need to check if 
                // user can read items: AccessMode.Read = 1

                List<ISolrQuery> filterQuery = new List<ISolrQuery>();

                if (!isAdmin && guids.Count > 0)
                {
                    // XXX For now work with read access mode, add others later

                    Guid publicGroup = AccessContext.PublicAccessGuid;

                    AbstractSolrQuery accessFilterQuery = new SolrQueryByField("access_1_ss", publicGroup.ToString());

                    foreach (Guid guid in guids)
                    {
                        accessFilterQuery += new SolrQueryByField("access_1_ss", guid.ToString());
                    }

                    //ISolrQuery[] filterQuery = new ISolrQuery[]
                    //{
                    //    filterOptions
                    //};

                    filterQuery.Add(accessFilterQuery);
                }

                if (!string.IsNullOrEmpty(entityTypeFilter))
                {
                    AbstractSolrQuery entitiTypeFilterQuery = new SolrQueryByField("entitytype_s", entityTypeFilter);
                    filterQuery.Add(entitiTypeFilterQuery);
                }
                
                options.FilterQueries = filterQuery;                

                SolrQueryResults<SolrIndex> results = solr.Query(solrQuery, options);
                total = results.NumFound;

                int resultsCount = results.Count();
                string query;

                if (resultsCount > 0)
                {
                    IEnumerable<string> values = results.Select((r, i) => {
                        return "(" + i + ",'" + r.SolrId + "')";
                    });
                    string valuesString = "values" + String.Join(",", values);

                    query = $@" 
                    SELECT cf.* FROM [dbo].[CFXmlModels] cf 
                    JOIN ({valuesString}) 
                    AS x (ordering, id) 
                    ON cf.MappedGuid = x.id
                    ORDER BY x.ordering ";
                }
                else
                {
                    // Produce an empty result
                    query = "SELECT * FROM [dbo].[CFXmlModels] WHERE Id < 0";
                }

                return set.SqlQuery(query).AsQueryable();

                //return set.Where(p => results.Contains(p.Id)); // the Contians method is slow because it creates several or expressions. 
                // More info can be found here: https://stackoverflow.com/questions/8107439/why-is-contains-slow-most-efficient-way-to-get-multiple-entities-by-primary-ke
            }

            throw new InvalidOperationException("The SolrService has not been initialized.");
        }

        public static IQueryable<TSource> FindAccessibleByGuid<TSource>(
           this DbSet<TSource> set,
           Guid guid,
           AccessMode mode = AccessMode.Read) where TSource : CFXmlModel
        {
            return FindAccessibleByGuid(set, new List<Guid> { guid }, mode);
        }

        public static IQueryable<TSource> FindAccessibleByGuid<TSource>(
        this DbSet<TSource> set,
        ICollection<Guid> guids,
        AccessMode mode = AccessMode.Read) where TSource : CFXmlModel
        {

            // publicGroup guid is all 0 and everyone is part of this group
            Guid publicGroup = AccessContext.PublicAccessGuid;
            guids.Add(publicGroup);

            string guidList = string.Join(",", Array.ConvertAll(guids.ToArray(), g => "'" + g + "'"));
            string sqlQuery = $@"
            SELECT *
            FROM
            (SELECT 
                CFXmlModels.*,
                CAST( [content] AS NVARCHAR(MAX) ) AS [contentCast],
                pref.value('access-definition[1]/access-modes[1]', 'int') AS Mode,
                pref.value('access-guid[1]', 'char(36)') AS Guid
                FROM CFXmlModels CROSS APPLY
                content.nodes('//access/access-group') AS contentCast(pref)
            ) AS Result
            WHERE Discriminator = '{(string)typeof(TSource).Name.ToString()}' 
            AND Mode & {(int)mode} = {(int)mode} 
            AND Guid IN ({(string)guidList})
            ";

            return set.SqlQuery(sqlQuery).AsQueryable().Distinct();
        }

        public static IQueryable<TSource> FindAccessible<TSource>(
            this DbSet<TSource> set,
            bool isAdmin,
            Guid guid,
            AccessMode mode) where TSource : CFXmlModel
        {
            return FindAccessible(set, isAdmin, new List<Guid> { guid }, mode);
        }

        public static IQueryable<TSource> FindAccessible<TSource>(
            this DbSet<TSource> set,
            bool isAdmin,
            ICollection<Guid> guids,
            AccessMode mode) where TSource : CFXmlModel
        {

            if (isAdmin)
            {
                return set.AsQueryable();
            }

            return FindAccessibleByGuid(set, guids, mode);
        }

    }


}

﻿using Catfish.Core.Contexts;
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


        /// <summary>
        /// Obtains a set of Models from the database based on a Solr Query.
        /// </summary>
        /// <typeparam name="TSource">The type of model to return.</typeparam>
        /// <param name="set">The datasaet to perform this query against.</param>
        /// <param name="solrQuery">The filter used to find matching data using the Solr index.</param>
        /// <param name="total">The resulting total number of entries matching the input criteria.</param>
        /// <param name="entityTypeFilter">A filter used to limit the entity types returned.</param>
        /// <param name="start">If paged, this is the starting result index to use.</param>
        /// <param name="rows">The total number of rows to return.</param>
        /// <param name="sortRowId">The row to sort the results by.</param>
        /// <param name="sortAscending">Whether or not the results should be sorted in ascending order.</param>
        /// <returns>An IQueryable object representing the search results.</returns>
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
               
                orderBy.AddLast(new SortOrder("name_mapping_s", Order.ASC));
               

                if (!string.IsNullOrEmpty(sortRowId))
                {
                    orderBy.AddFirst(new SortOrder(sortRowId, sortAscending ? Order.ASC : Order.DESC));
                }

                options.OrderBy = orderBy;

                options.Fields = SolrIndex.Fields;

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

                string query;

                if (total > 0)
                {
                    IEnumerable<string> values = results.Select((r, i) => {
                        return "(" + i + ",'" + r.SolrId + "')";
                    });

                    query = $@" 
                    SELECT cf.Discriminator as Discriminator, cf.* 
                    FROM (values {String.Join(",", values)}) AS x (ordering, id) 
                    JOIN [dbo].[CFXmlModels] cf 
                    ON cf.MappedGuid = x.id
                    ORDER BY x.ordering ASC";
                }
                else
                {
                    // Produce an empty result
                    query = "SELECT * FROM [dbo].[CFXmlModels] WHERE Id < 0";
                }

                //var test = set.Where(c => c.Id != 0);
                //var testQuest = test.AsQueryable();

                return set.SqlQuery(query).AsQueryable();

                //IEnumerable<string> ids = results.Select(r => r.SolrId);
                //return set.Where(p => ids.Contains(p.MappedGuid)); // the Contians method is slow because it creates several or expressions. 
                // More info can be found here: https://stackoverflow.com/questions/8107439/why-is-contains-slow-most-efficient-way-to-get-multiple-entities-by-primary-ke
            }

            throw new InvalidOperationException("The SolrService has not been initialized.");
        }

        /// <summary>
        /// Find if an item in the database is accessable given the supplied guid and Access Modes.
        /// </summary>
        /// <typeparam name="TSource">The type of object to obtain.</typeparam>
        /// <param name="set">The set to perform the query against.</param>
        /// <param name="guid">The User or User List guid to compare against.</param>
        /// <param name="mode">The AccessModes to validate against.</param>
        /// <returns>All accessable elements based on the given input.</returns>
        public static IQueryable<TSource> FindAccessibleByGuid<TSource>(
           this DbSet<TSource> set,
           Guid guid,
           AccessMode mode = AccessMode.Read) where TSource : CFXmlModel
        {
            return FindAccessibleByGuid(set, new List<Guid> { guid }, mode);
        }

        /// <summary>
        /// Find if an item in the database is accessable given the supplied guids and Access Modes.
        /// </summary>
        /// <typeparam name="TSource">The type of object to obtain.</typeparam>
        /// <param name="set">The set to perform the query against.</param>
        /// <param name="guids">The list of User or User List guids to compare against.</param>
        /// <param name="mode">The AccessModes to validate against.</param>
        /// <returns>All accessable elements based on the given input.</returns>
        public static IQueryable<TSource> FindAccessibleByGuid<TSource>(
        this DbSet<TSource> set,
        ICollection<Guid> guids,
        AccessMode mode = AccessMode.Read) where TSource : CFXmlModel
        {

            // publicGroup guid is all 0 and everyone is part of this group
            Guid publicGroup = AccessContext.PublicAccessGuid;
            guids.Add(publicGroup);
             
            string strDiscriminator = "";
            if(typeof(TSource).Name.ToString() == "CFAggregation")
            {
                List<string> subClasses = DiscriminatorHelper.GetDiscriminatorWhere<CFAggregation>();
                strDiscriminator = "";
                for (int i=0; i < subClasses.ToArray().Length; i++)
                {  
                    strDiscriminator += " Discriminator ='" + subClasses[i] + "' ";
                    if (i < (subClasses.ToArray().Length - 1))
                        strDiscriminator += " OR ";
                }

            }
            else
            {
                strDiscriminator = "Discriminator = '" + typeof(TSource).Name.ToString() + "' ";
            }

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
            WHERE {strDiscriminator} 
            AND Mode & {(int)mode} = {(int)mode} 
            AND Guid IN ({(string)guidList})
            ";

            return set.SqlQuery(sqlQuery).AsQueryable().Distinct();
        }

        /// <summary>
        /// Find if an item in the database is accessable given the supplied guid and Access Modes.
        /// </summary>
        /// <typeparam name="TSource">The type of object to obtain.</typeparam>
        /// <param name="set">The set to perform the query against.</param>
        /// <param name="isAdmin">Whether or not the current guid is an admin.</param>
        /// <param name="guid">The User or User List guid to compare against.</param>
        /// <param name="mode">The AccessModes to validate against.</param>
        /// <returns>All accessable elements based on the given input.</returns>
        public static IQueryable<TSource> FindAccessible<TSource>(
            this DbSet<TSource> set,
            bool isAdmin,
            Guid guid,
            AccessMode mode) where TSource : CFXmlModel
        {
            return FindAccessible(set, isAdmin, new List<Guid> { guid }, mode);
        }

        /// <summary>
        /// Find if an item in the database is accessable given the supplied guids and Access Modes.
        /// </summary>
        /// <typeparam name="TSource">The type of object to obtain.</typeparam>
        /// <param name="set">The set to perform the query against.</param>
        /// <param name="isAdmin">Whether or not the current guid is an admin.</param>
        /// <param name="guids">The list of User or User List guids to compare against.</param>
        /// <param name="mode">The AccessModes to validate against.</param>
        /// <returns>All accessable elements based on the given input.</returns>
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

using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Solr;
using Showtimes.API.DTO;
using System.Linq;

namespace Showtimes.API.Services
{

    public delegate void Action();
    
    public static class BackgroundProcessingDelegate
    {
        private static readonly ISolrService _solr;

        public static void QueryDelegate(Action action)
        {
            action();
        }
        public static async Task NotifyUser(string requestLabel, string notificationEmail)
        {
            string message = "Your query for " + requestLabel + " is ready.";
            //send notification to user
        }
        public static void CountShowtimes(QueryParams param,out int count)
        {
            // solrSearchResult = await _solr.ExecuteSearch(query, offset, max, filterQuery, sortBy, fieldList, maxHiglightSnippets);
            // var jobId = BackgroundJob.Enqueue(() => _solr.ExecuteSearch());
            string query= BuildingQueryString(param);
            //await _solr.ExecuteSearch(query, offset, max, filterQuery, sortBy, fieldList, maxHiglightSnippets);
            if (query == null)
                throw new Exception("In valid query string");

            var solrSearchResult = _solr.ExecuteSearch(query, 0, 1000000).Result;
            count = solrSearchResult.TotalMatches;
            //return solrSearchResult.TotalMatches;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryParams">
        /// QueryParam: public string? countryCode;
        ///public int? movideId;
        ///public int? theaterId;
        /// public DateTime? fromDate;
        ///public DateTime? toDate;
        /// </param>
        /// <returns></returns>
        public static string? BuildingQueryString(QueryParams queryParams)
        {
            //TODO: Solr fields name
            string countryFieldName = "";
            string movieIdFieldName = "";
            string theaterIdFieldName = "";
            string fromDateFieldName = "";
            string toDateFieldName = "";

            List<string> queries = new List<string>();
            if (queryParams == null)
                return null;

            if (!string.IsNullOrEmpty(queryParams.countryCode))
                queries.Add(countryFieldName + ":\"" + queryParams.countryCode + "\"");

            if (queryParams.movideId != null && queryParams.movideId > 0)
                queries.Add(movieIdFieldName + ":" + queryParams.movideId);

            if (queryParams.theaterId != null && queryParams.theaterId > 0)
                queries.Add(theaterIdFieldName + ":" + queryParams.theaterId);

            if (!string.IsNullOrEmpty(queryParams.fromDate.ToString()))
                queries.Add(fromDateFieldName + ":\"" + queryParams.fromDate.ToString() + "\"");

            if (!string.IsNullOrEmpty(queryParams.toDate.ToString()))
                queries.Add(toDateFieldName + ":\"" + queryParams.toDate.ToString() + "\"");

            return string.Join("AND", queries.ToArray());
        }
    }
}

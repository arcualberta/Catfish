using Showtimes.API.DTO;
using Showtimes.API.Interfaces;
using Catfish.API.Repository.Interfaces;

namespace Showtimes.API.Services
{
    public class ShowtimeQueryService : IShowtimeQueryService
    {
        private  readonly ISolrService _solr;
        private  readonly IEmailService _emailSrv;
        public string? BuildingQueryString(QueryParams queryParams)
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

        public void CountShowtimes(QueryParams param, out int count)
        {
            count = Task.Run(() => CountShowtimesAsync(param)).GetAwaiter().GetResult(); 
        }

        public async Task<int> CountShowtimesAsync(QueryParams param)
        {
            // solrSearchResult = await _solr.ExecuteSearch(query, offset, max, filterQuery, sortBy, fieldList, maxHiglightSnippets);
            // var jobId = BackgroundJob.Enqueue(() => _solr.ExecuteSearch());
            string query = BuildingQueryString(param);
            //await _solr.ExecuteSearch(query, offset, max, filterQuery, sortBy, fieldList, maxHiglightSnippets);
            if (query == null)
                throw new Exception("In valid query string");

            var solrSearchResult = await _solr.ExecuteSearch(query, 0, 1000000);
            return solrSearchResult.TotalMatches;
        }

        public void NotifyUser(string requestLabel, string notificationEmail)
        {
            try
            {
                string message = "Your query for " + requestLabel + " is ready.";
                CatfishExtensions.DTO.Email emailDto = new CatfishExtensions.DTO.Email();
                emailDto.Body = message;
                emailDto.Subject = "Showtimes Query Request";

                emailDto.ToRecipientEmail = new List<string> { notificationEmail };

                _emailSrv.SendEmail(emailDto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}

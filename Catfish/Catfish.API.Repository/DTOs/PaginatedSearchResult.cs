using Catfish.API.Repository.Models.BackgroundJobs;
using Catfish.API.Repository.Solr;

namespace Catfish.API.Repository.DTOs
{
    public class PaginatedSearchResult
    {
        public int Offset { get; set; }

        /// <summary>
        /// The total number of matches found, which may be larger than the page size.
        /// </summary>
        public int TotalMatches { get; set; }

        public int ItemsPerPage { get; set; }
    }
}

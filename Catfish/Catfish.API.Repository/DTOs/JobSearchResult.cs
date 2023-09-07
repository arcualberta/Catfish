using Catfish.API.Repository.Models.BackgroundJobs;
using Catfish.API.Repository.Solr;

namespace Catfish.API.Repository.DTOs
{
    public class JobSearchResult: PaginatedSearchResult
    {
        public List<JobRecord> ResultEntries { get; set; }
    }
}

using Catfish.API.Repository.DTOs;
using Catfish.API.Repository.Models.BackgroundJobs;

namespace Catfish.API.Repository.Interfaces
{
    public interface IBackgroundJobService
    {
        public void  DummyTest();
        public string RunTestBackgroundJob();

        public Task<JobSearchResult> GetJobs(int offset, int max, string? searchTerm = null, bool inProgressOnly = false);
        public Task<JobRecord?> GetJob(Guid id);

        public Task RemoveBackgroundJob(Guid jobId);
    }
}

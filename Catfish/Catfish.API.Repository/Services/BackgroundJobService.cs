using Catfish.API.Repository.DTOs;
using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Models.BackgroundJobs;
using Catfish.API.Repository.Models.Forms;
using Catfish.API.Repository.Models.Workflow;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Catfish.API.Repository.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly RepoDbContext _db;
        public BackgroundJobService(RepoDbContext db)
        {
            _db = db;
        }

        public void DummyTest()
        {
             string folderRoot = "C:\\Projects\\HangfireLogs";
              if (!(System.IO.Directory.Exists(folderRoot)))
                  System.IO.Directory.CreateDirectory(folderRoot);

              string logFile = Path.Combine(folderRoot, "hangfireTest.txt");
              if (!File.Exists(logFile))
                  File.Create(logFile).Close();

              for (int i=1; i<=200; i++)
              {
                  File.AppendAllText(logFile, $"writeline : {i}.{Environment.NewLine}");
                  Thread.Sleep(1000);
              } 
        }

        public async Task<JobSearchResult> GetJobs(int offset, int max, string? searchTerm=null, bool inProgressOnly = false)
        {
            JobSearchResult result = new()
            {
                Offset = offset
            };

            IQueryable<JobRecord> query = _db.JobRecords.Where(j => j.IsDeleted != true);


            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(j => j.JobLabel.Contains(searchTerm));

            if (inProgressOnly)
                query = query.Where(entry => entry.Status == "In Progress");


            result.ResultEntries = await query.OrderByDescending(rec => rec.Started).Skip(offset).Take(max).ToListAsync();


            ////if (!string.IsNullOrEmpty(searchTerm))
            ////{
            ////    result.ResultEntries = await _db.JobRecords.Where(j => j.JobLabel.Contains(searchTerm) && j.IsDeleted != true).OrderByDescending(rec => rec.Started).Skip(offset).Take(max).ToListAsync();
            ////    result.TotalMatches = await _db.JobRecords.Where(j => j.JobLabel.Contains(searchTerm) && j.IsDeleted != true).CountAsync();
            ////}
            ////else 
            ////{
            ////    result.ResultEntries = await _db.JobRecords.Where(j=>j.IsDeleted != true).OrderByDescending(rec => rec.Started).Skip(offset).Take(max).ToListAsync();
            ////    result.TotalMatches = await _db.JobRecords.Where(j => j.IsDeleted != true).CountAsync();
            ////}

            //////Out of the paginated result set, select the sub-set of entries that are still in progress.
            ////if (inProgressOnly)
            ////    result.ResultEntries = result.ResultEntries.Where(entry => entry.Status == "In Progress").ToList();

            return result;
        }

        public async Task<JobRecord?> GetJob(Guid id)
        {
            return await _db.JobRecords.FirstOrDefaultAsync(x => x.Id == id);
        }

        public string RunTestBackgroundJob()
        {
            var jobId = BackgroundJob.Enqueue(() => DummyTest());

            Console.WriteLine($"Hangfire is processing job id: {jobId}");

            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine($"{jobId} is done ."));
            return jobId;
        }

        public async Task RemoveBackgroundJob(Guid jobId)
        {
            try
            {
                JobRecord jobRecord = await _db.JobRecords.FindAsync(jobId);

                if (jobRecord == null)
                    throw new Exception("Object not found");

                //stop hangfire job
                if(!string.IsNullOrEmpty(jobRecord.JobId))
                    BackgroundJob.Delete(jobRecord.JobId);

                jobRecord.IsDeleted = true;
                jobRecord.DeletedDate = DateTime.UtcNow;

                _db.Entry(jobRecord).State = EntityState.Modified;

                _db.SaveChanges();


               
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}

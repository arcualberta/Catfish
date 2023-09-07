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

        public async Task<JobSearchResult> GetJobs(int offset, int max)
        {
            JobSearchResult result = new JobSearchResult()
            {
                Offset = offset,
                TotalMatches = await _db.JobRecords.CountAsync()
            };

            result.ResultEntries = await _db.JobRecords.OrderByDescending(rec => rec.Started).Skip(offset).Take(max).ToListAsync();

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

    }
}

using Catfish.API.Repository.Interfaces;
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
          
        public string RunTestBackgroundJob()
        {
            var jobId = BackgroundJob.Enqueue(() => DummyTest());

            Console.WriteLine($"Hangfire is processing job id: {jobId}");
            return jobId;
        }
    }
}

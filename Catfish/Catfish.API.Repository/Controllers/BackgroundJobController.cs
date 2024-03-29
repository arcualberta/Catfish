﻿using Catfish.API.Repository.DTOs;
using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Models.BackgroundJobs;

namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route(Routes.BackgroundJob.Root)]
    public class BackgroundJobController : ControllerBase
    {
        //private readonly RepoDbContext _context;
        private readonly IBackgroundJobService _bgJobSrv;
        public BackgroundJobController(IBackgroundJobService bgJobSrv)
        {
            _bgJobSrv = bgJobSrv;
        }
        // GET: api/Forms
        [HttpGet]
        public async Task<JobSearchResult> Get(int offset = 0, int max = 100, string? searchTerm = null)
        {
            return await _bgJobSrv.GetJobs(offset, max, searchTerm);
        }

        [HttpGet("job")]
        public async Task<JobRecord> GetJobRecord(Guid id)
        {
            return await _bgJobSrv.GetJob(id);
        }


        [HttpPost("remove-job")]
        public async Task RemoveJob(Guid jobId)
        {
            //markedjob as  deleted
           await _bgJobSrv.RemoveBackgroundJob(jobId);  
           
        }
    }

}

using Catfish.API.Repository.Interfaces;

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
           // _context = context;
            _bgJobSrv = bgJobSrv;
        }
        // GET: api/Forms
        [HttpGet]
        public async Task<string> Get()
        {
            return _bgJobSrv.RunTestBackgroundJob();
        }
      
    }
}

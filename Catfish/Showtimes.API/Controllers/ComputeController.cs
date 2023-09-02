using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Showtimes.API.DTO;
using Showtimes.API.Interfaces;
using Showtimes.API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Showtimes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputeController : ControllerBase
    {
        private readonly IShowtimeQueryService _showtimeQuery;
        public ComputeController(IShowtimeQueryService showtimeQuery)
        {
            _showtimeQuery = showtimeQuery;
        }

        [HttpPost("showtimes-count")]
        public async Task<ActionResult> ShowtimesCount(string requestLabel, string notificationEmail, QueryParams queryParams)
        {
            //TODO:
            // Create and save the request object
            // Send the request to Catfish batch processing
            // Return the Guid of the created request object

            //Using multicast Delegate -- option 1
            int count = 0;
       
            BackgroundProcessingDelegate.QueryDelegate(delegate { BackgroundProcessingDelegate.CountShowtimes(queryParams, out count); });
            BackgroundProcessingDelegate.QueryDelegate(delegate { BackgroundProcessingDelegate.NotifyUser(requestLabel, notificationEmail); });

            //Hangfire BGJob -- option 2 -- I like this option 
            var parentJobId = BackgroundJob.Enqueue(() => _showtimeQuery.CountShowtimes(queryParams, out count));
            BackgroundJob.ContinueJobWith(parentJobId, () => _showtimeQuery.NotifyUser(requestLabel, notificationEmail));

            return Ok();
        }

        public async Task<List<ReponseInfoDto>> GetResponses(Guid requestId)
        {
            //TODO:
            // Get the list of respnses associated with the specified request and send them

            return new List<ReponseInfoDto>();
        }
    }
}

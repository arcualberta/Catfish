using Microsoft.AspNetCore.Mvc;
using Showtimes.API.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Showtimes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComputeController : ControllerBase
    {
        [HttpPost("showtimes-count")]
        public async Task<ActionResult> ShowtimesCount(string requestLabel, string notificationEmail, QueryParams queryParams)
        {
            //TODO:
            // Create and save the request object
            // Send the request to Catfish batch processing
            // Return the Guid of the created request object
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

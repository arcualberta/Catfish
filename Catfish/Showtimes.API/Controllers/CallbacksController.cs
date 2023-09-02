using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Showtimes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbacksController : ControllerBase
    {
        [HttpPost("save-value")]
        public async Task<ActionResult> SaveValue(Guid requestId, string resultValue)
        {
            return Ok();
        }

        [HttpPost("save-file")]
        public async Task<ActionResult> SaveFile(Guid requestId, IFormFile resultFile)
        {
            return Ok();
        }

    }
}

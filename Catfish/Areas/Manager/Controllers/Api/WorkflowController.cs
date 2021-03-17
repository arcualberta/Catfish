using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Controllers.Api
{
    [Route("manager/api/[controller]")]
    [ApiController]
    public class WorkflowController : Controller
    {
        public JsonResult SaveText(Guid templateId, Guid dataItemId, Guid fieldId, Guid textId, string value)
        {
            throw new NotImplementedException();
        }
    }
}

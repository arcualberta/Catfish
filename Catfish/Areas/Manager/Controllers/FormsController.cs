using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Services.FormBuilder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Catfish.Areas.Manager.Controllers
{
    [Route("manager/apiold/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly IFormService _service;

        public FormsController(IFormService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Get(int offset = 0, int? max = null)
        {
            FieldContainerListVM vm = _service.Get(offset, max);
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string jsonString = JsonConvert.SerializeObject(vm, settings);
            return Content(jsonString, "application/json");
        }

        // GET: api/Forms/[ID]
        [HttpGet("{id}")]
        public ActionResult Get(Guid id)
        {
            Form data = _service.Get(id) as Form;
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string jsonString = JsonConvert.SerializeObject(data, settings);
            return Content(jsonString, "application/json");
        }

        [Route("fielddefs")]
        public ActionResult FieldDefs()
        {
            List<BaseField> fields = _service.GetFieldDefinitions();
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string jsonString = JsonConvert.SerializeObject(fields, settings);
            return Content(jsonString, "application/json");
        }

    }
}
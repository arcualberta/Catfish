using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Services.FormBuilder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catfish.Areas.Manager.Controllers
{
    [Route("manager/api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly IFormService _service;

        public FormsController(IFormService service)
        {
            _service = service;
        }

        [HttpGet]
        public FieldContainerListVM Get(int offset = 0, int? max = null)
        {
            FieldContainerListVM vm = _service.Get(offset, max);
            return vm;
        }

        // GET: api/Forms/[ID]
        [HttpGet("{id}")]
        public Form Get(Guid id)
        {
            return _service.Get(id) as Form;
        }

    }
}
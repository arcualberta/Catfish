using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catfish.Areas.Manager.Controllers
{
    [Route("manager/api/[controller]")]
    [ApiController]
    public class MetadataSetsController : ControllerBase
    {
        private readonly IFormsService _formService;

        public MetadataSetsController(IFormsService formSrv)
        {
            _formService = formSrv;
        }

        [HttpGet]
        public FieldContainerListVM Get(int offset = 0, int max = 0)
        {
            FieldContainerListVM vm = _formService.GetMetadataSets(offset, max);
            return vm;
        }
    }
}
using Catfish.Core.Models;

using Catfish.Core.Models.Contents.Fields;
using Catfish.Core.Models.Contents.ViewModels;
using Catfish.Core.Services.FormBuilder;
using Catfish.Models.FormBuilder;
using Catfish.Models.FormBuilder.Fields;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Areas.Manager.Controllers.Api
{
    [Route("manager/api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly IFormService _service;
        private readonly AppDbContext _appDb;
        public FormsController(IFormService service, AppDbContext appDb)
        {
            _service = service;
            _appDb = appDb;
        }

        // GET: api/<FormsController>
        [HttpGet]
        public FieldContainerListVM Get(int offset = 0, int? max = null)
        {
            FieldContainerListVM vm = _service.Get(offset, max);
            return vm;
            //JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            //string jsonString = JsonConvert.SerializeObject(vm, settings);
            //return Content(jsonString, "application/json");
        }

        // GET api/<FormsController>/5
        [HttpGet("{id}")]
        public Form Get(Guid id)
        {
            Form form = new Form();

            if (id != Guid.Empty)
            {
                Core.Models.Contents.FieldContainer _form = _service.Get(id);
                form.UpdateViewModel(_form);
            }

            return form;
        }

        /// <summary>
        ///  create/edit form (Catfish.Core.Models.Contents.Form)
        /// </summary>
        /// <param name="form">Catfish.Models.FormBuilder.Form</param>
        // POST api/<FormsController>
        [HttpPost]
        public object Post([FromBody] Form viewModel)
        {
            try
            {
                Core.Models.Contents.Form dataModel = _appDb.Forms.FirstOrDefault(f => f.Id == viewModel.Id);
                bool created = false;
                if (dataModel == null)
                {
                    dataModel = viewModel.CreateDataModel();
                    _appDb.Forms.Add(dataModel);
                    created = true;
                }
                else
                {
                    dataModel.Initialize(XmlModel.eGuidOption.Ignore);
                    viewModel.UpdateDataModel(dataModel);
                }

                _appDb.SaveChanges();

                return new { id = dataModel.Id, created = created };
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        // PUT api/<FormsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string formStr)
        {
        }

        // DELETE api/<FormsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        //[HttpGet]
        //public void Edit(Guid? id)
        //{
        //    Get(id);
        //}
    }
}

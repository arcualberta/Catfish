﻿using Catfish.Core.Models;

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
        [Route("{FormsController}/edit")]
        [Route("{FormsController}/{id}")]
        public Form Get(Guid? id=null)
        {
           
            // MR temporary set the fid to some random integer
            //need to change ForeignKey to "Guid"??
            Random rnd = new Random();
            int fid = rnd.Next(1, 1000);

            Form form = new Form()
            {
                ForeignId = fid,
                Name = "Test Form",
                Description = "This is an example form hardcoded for testing purposes.",
                LinkText = "Link Text"
                
            };
            int fidSeed = 100 * fid;

            if (id != null )
            {
                 Core.Models.Contents.FieldContainer _form = _service.Get(id.Value);
              
                form.UpdateViewModel(_form);
              
            }
            else
            {
                form.AppendField<ShortText>(++fidSeed, "First Name", "", true);
                form.AppendField<ShortText>(++fidSeed, "Last Name", "", true);
                form.AppendField<EmailAddress>(++fidSeed, "Email", "Enter a valid email address to send receipts and correspondence.", true);
                form.AppendField<RadioButtonSet>(++fidSeed, "Subscribe to newsletter", new string[] { "Yes", "No" });

                NumberField tickets = form.AppendField<NumberField>(++fidSeed, "Number of Tickets", "", true);

                form.AppendField<LongText>(++fidSeed, "Meal Preferences", "Please specify any meal preferences or restrictions.", true);
            }
            return form;
        }

        /// <summary>
        ///  create/edit form (Catfish.Core.Models.Contents.Form)
        /// </summary>
        /// <param name="form">Catfish.Models.FormBuilder.Form</param>
        // POST api/<FormsController>
        [HttpPost]
        public void Post([FromBody] Form viewModel)
        {
            try
            {
                Core.Models.Contents.Form dataModel = _appDb.Forms.FirstOrDefault(f => f.Id == viewModel.Id);

                if (dataModel == null)
                {
                    dataModel = viewModel.CreateDataModel();
                    _appDb.Forms.Add(dataModel);
                }
                else
                {
                    dataModel.Initialize(XmlModel.eGuidOption.Ignore);
                    viewModel.UpdateDataModel(dataModel);
                }

                _appDb.SaveChanges();
            }
            catch (Exception ex)
            {

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

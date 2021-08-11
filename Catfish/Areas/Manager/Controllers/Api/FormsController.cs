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
        public Form Get(int id)
        {
            int fid = id ;
            Form form = new Form()
            {
                ForeignId = fid,
                Name = "Test Form",
                Description = "This is an example form hardcoded for testing purposes."
            };
            int fidSeed = 100 * fid;
            form.AppendField<ShortText>(++fidSeed, "First Name", "", true);
            form.AppendField<ShortText>(++fidSeed, "Last Name", "", true);
            form.AppendField<EmailAddress>(++fidSeed, "Email", "Enter a valid email address to send receipts and correspondence.", true);
            form.AppendField<RadioButtonSet>(++fidSeed, "Subscribe to newsletter", new string[] { "Yes", "No" });

            NumberField tickets = form.AppendField<NumberField>(++fidSeed, "Number of Tickets", "", true);

            form.AppendField<LongText>(++fidSeed, "Meal Preferences", "Please specify any meal preferences or restrictions.", true);

            return form;
        }

        /// <summary>
        ///  create/edit form (Catfish.Core.Models.Contents.Form)
        /// </summary>
        /// <param name="form">Catfish.Models.FormBuilder.Form</param>
        // POST api/<FormsController>
        [HttpPost]
        public void Post([FromBody] Form form)
        {
            try
            {
                Catfish.Core.Models.Contents.Form _form = new Core.Models.Contents.Form();
                string lang = "en";

                _form.Initialize(Core.Models.XmlModel.eGuidOption.Ensure);

                _form.SetName(form.Name, lang);
                _form.SetDescription(form.Description, lang);
                foreach (Field fld in form.Fields)
                {
                    switch (fld.ComponentType)
                    {
                        case "LongText":
                            _form.CreateField<TextArea>(fld.Name, lang, fld.IsRequired).SetDescription(fld.Description, lang);
                            break;
                        case "EmailAddress":
                            _form.CreateField<EmailField>(fld.Name, lang, fld.IsRequired).SetDescription(fld.Description, lang);
                            break;
                        case "NumberField":
                            _form.CreateField<IntegerField>(fld.Name, lang, fld.IsRequired).SetDescription(fld.Description, lang);
                            break;
                        case "RadioButtonSet":
                        
                            List<string> optionTexts = new List<string>();

                            foreach (var op in (fld as OptionField).Options)
                                optionTexts.Add(op.Label);

                            _form.CreateField<RadioField>(fld.Name, lang, optionTexts.ToArray(),fld.IsRequired).SetDescription(fld.Description, lang);
                            break;
                        case "CheckBoxSet":
                            List<string> options = new List<string>();
                            options.Clear();

                            foreach (var op in (fld as OptionField).Options)
                                options.Add(op.Label);

                            _form.CreateField<CheckboxField>(fld.Name, lang, options.ToArray(), fld.IsRequired).SetDescription(fld.Description, lang);
                            break;
                        case "DropDownField":
                            List<string> ddoptions = new List<string>();
                            

                            foreach (var op in (fld as OptionField).Options)
                                ddoptions.Add(op.Label);

                            _form.CreateField<SelectField>(fld.Name, lang, ddoptions.ToArray(), fld.IsRequired).SetDescription(fld.Description, lang);
                            break;
                        default://shortText
                            _form.CreateField<TextField>(fld.Name,lang, fld.IsRequired).SetDescription(fld.Description, lang);
                            break;

                    }
                    
                  
                }
                _appDb.Forms.Add(_form);
                _appDb.SaveChanges();

            }catch(Exception ex)
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
    }
}

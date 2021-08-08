using Catfish.Models.FormBuilder;
using Catfish.Models.FormBuilder.Fields;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        // GET: api/<FormsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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

        // POST api/<FormsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FormsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FormsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

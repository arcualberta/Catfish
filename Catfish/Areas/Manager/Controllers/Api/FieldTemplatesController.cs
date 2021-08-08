using Catfish.Models.FormBuilder;
using Catfish.Models.FormBuilder.Fields;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Catfish.Areas.Manager.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldTemplatesController : ControllerBase
    {
        [HttpGet]
        public List<Field> Index()
        {
            List<Field> templates = new List<Field>();
            templates.Add(new ShortText() { Icon="fas fa-edit"});
            templates.Add(new LongText(){ Icon = "fas fa-paragraph" });
            templates.Add(new EmailAddress() {Icon="fas fa-envelope-open-text" });
            templates.Add(new CheckboxSet() { Icon = "fas fa-check-square" });
            templates.Add(new DropDownMenu(){ Icon = "fas fa-caret-square-down" });
            templates.Add(new NumberField() { Icon = "fas fa-sort-numeric-up" });
            templates.Add(new RadioButtonSet() { Icon = "fas fa-adjust" });
            return templates;
        }
    }
}

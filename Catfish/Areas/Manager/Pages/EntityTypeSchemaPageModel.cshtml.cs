using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Catfish.Core.Models;
using ElmahCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Areas.Manager.Pages
{
    public class EntityTypeSchemaPageModel : PageModel
    {
        [BindProperty]
        [DataType(DataType.MultilineText)]
        public string SchemaXml { get; set; }

        public string ErrorMessage { get; set; } = null;
        public string SuccessMessage { get; set; } = null;

        private AppDbContext _db;
        private ErrorLog _errorLog;

        public EntityTypeSchemaPageModel(AppDbContext db, ErrorLog errorLog)
        {
            _db = db;
            _errorLog = errorLog;
        }

        public void OnGet(Guid id)
        {
            EntityTemplate template = _db.EntityTemplates.Where(et => et.Id == id).FirstOrDefault();
            SchemaXml = template != null ? template.Content : "";
        }

        public IActionResult OnPost(Guid id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SchemaXml))
                    throw new Exception("The schema cannot be empty");

                //Make sure the schemaXML represents a valid xml string
                XElement xml = XElement.Parse(SchemaXml);
                EntityTemplate template = _db.EntityTemplates.Where(et => et.Id == id).FirstOrDefault();
                if (template == null)
                {
                    string typeString = xml.Attribute("model-type").Value;
                    var type = Type.GetType(typeString);
                    template = Entity.Parse(xml, true) as EntityTemplate;
                    _db.EntityTemplates.Add(template);
                    id = template.Id;
                }
                else
                {
                    template.Content = SchemaXml;
                    template.TemplateName = template.Name.GetConcatenatedContent(" | ");
                    template.Updated = DateTime.Now;
                }

                _db.SaveChanges();

                SuccessMessage = "Schema saved successfully.";
            }
            catch(Exception ex)
            {
                _errorLog.Log(new Error(ex));
                ErrorMessage = ex.Message;
            }

            return RedirectToPage(new { id });
        }

    }
}

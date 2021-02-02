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
    public class SchemaPageModel : PageModel
    {
        [BindProperty]
        [DataType(DataType.MultilineText)]
        public string SchemaXml { get; set; }

        public string ErrorMessage { get; set; } = null;
        public string SuccessMessage { get; set; } = null;

        private AppDbContext _db;
        private ErrorLog _errorLog;

        public SchemaPageModel(AppDbContext db, ErrorLog errorLog)
        {
            _db = db;
            _errorLog = errorLog;
        }

        public void OnGet(Guid id, string successMessage)
        {
            Entity entity = _db.Entities.Where(et => et.Id == id).FirstOrDefault();
            SchemaXml = entity != null ? entity.Content : "";
            SuccessMessage = successMessage;
        }

        public IActionResult OnPost(Guid id)
        {
            string successMessage = null;
            try
            {
                if (string.IsNullOrWhiteSpace(SchemaXml))
                    throw new Exception("The schema cannot be empty");

                //Make sure the schemaXML represents a valid xml string
                XElement xml = XElement.Parse(SchemaXml);
                Entity entity = _db.Entities.Where(et => et.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    string typeString = xml.Attribute("model-type").Value;
                    var type = Type.GetType(typeString);
                    entity = Entity.Parse(xml, true) as Entity;
                    _db.Entities.Add(entity);
                    id = entity.Id;
                }
                else
                {
                    entity.Content = SchemaXml;
                    if(typeof(EntityTemplate).IsAssignableFrom(entity.GetType()))
                        (entity as EntityTemplate).TemplateName = (entity as EntityTemplate).Name.GetConcatenatedContent(" | ");
                    entity.Updated = DateTime.Now;
                }

                _db.SaveChanges();

                successMessage = "Schema saved successfully.";
                return RedirectToPage(new { id, successMessage });
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                ErrorMessage = ex.Message;
                return null;
            }
        }

    }
}

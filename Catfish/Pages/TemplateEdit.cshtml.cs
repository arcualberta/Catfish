using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Catfish.Core.Models;
using ElmahCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    [Authorize(Roles = "SysAdmin")]
    public class TemplateEditModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly ErrorLog _errorLog;

        [BindProperty]
        public string TemplateContent { get; set; }

        [BindProperty]
        public string TemplateName { get; set; }

        [BindProperty]
        public Guid TemplateId { get; set; }
        public string Message { get; set; }
        public string MessageClass { get; set; }
        public TemplateEditModel(AppDbContext db, ErrorLog errorLog)
        {
            _db = db;
            _errorLog = errorLog;
        }
        public void OnGet(Guid id)
        {
            EntityTemplate template = _db.EntityTemplates
                .Where(t => t.Id == id)
                .FirstOrDefault();

            TemplateContent = template.Content;
            TemplateName = template.TemplateName;
            TemplateId = template.Id;
        }

        public void OnPost(Guid id)
        {
            try
            {
                //Making sure the passed on TemplateContent represents valid XML
                XElement data = XElement.Parse(TemplateContent);

                EntityTemplate template = _db.EntityTemplates.Where(t => t.Id == TemplateId).FirstOrDefault();
                if (template == null)
                    throw new Exception("Specified entity template could not be found.");

                template.TemplateName = TemplateName;
                template.Content = TemplateContent;
                _db.SaveChanges();

                Message = "Template saved successfully";
                MessageClass = "alert-success";
            }
            catch (Exception ex)
            {
                Message = "Save failed.\n\n" + ex.Message + "\n\n" + ex.StackTrace;
                MessageClass = "alert-danger";
                _errorLog.Log(new Error(ex));
            }
        }

    }
}

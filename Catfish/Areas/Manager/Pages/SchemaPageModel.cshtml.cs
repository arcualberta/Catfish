using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Catfish.Core.Models;
using Catfish.Core.Services;
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
        private IWorkflowService _workflowService;
        private IAuthorizationService _authorizationService;

        public SchemaPageModel(AppDbContext db, ErrorLog errorLog, IWorkflowService workflowService, IAuthorizationService authorizationService)
        {
            _db = db;
            _errorLog = errorLog;
            _workflowService = workflowService;
            _authorizationService = authorizationService;
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
                    entity.Updated = DateTime.Now;
                }

                List<string> oldGuids = new List<string>();
                List<string> newGuids = new List<string>();
                if (typeof(EntityTemplate).IsAssignableFrom(entity.GetType()))
                {
                    EntityTemplate template = entity as EntityTemplate;
                    template.TemplateName = (entity as EntityTemplate).Name.GetConcatenatedContent(" | ");
                    if (template.Workflow != null)
                    {

                        //Making sure the state values defined in the workflow matches with state values stored in 
                        //the database (and creating new state values in the database if matching ones are not available.
                        foreach (var state in template.Workflow.States)
                        {
                            var dbState = _workflowService.GetStatus(template.Id, state.Value, true);
                            if(state.Id != dbState.Id)
                            {
                                oldGuids.Add(state.Id.ToString());
                                newGuids.Add(dbState.Id.ToString());
                            }
                        }

                        //Making sure the roles defined in the workflow matches with roles stored in 
                        //the database (and creating new roles in the database if matching ones are not available.
                        foreach(var role in template.Workflow.Roles)
                        {
                            var dbRole = _authorizationService.GetRole(role.Value, true);
                            if(role.Id != dbRole.Id)
                            {
                                oldGuids.Add(role.Id.ToString());
                                newGuids.Add(dbRole.Id.ToString());
                            }
                        }
                    }
                }

                //Globally replace all oldGuids in the schema content with the corresponding newGuids.
                for (int i = 0; i < oldGuids.Count; ++i)
                    entity.Content = entity.Content.Replace(oldGuids[i], newGuids[i], StringComparison.InvariantCultureIgnoreCase);
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

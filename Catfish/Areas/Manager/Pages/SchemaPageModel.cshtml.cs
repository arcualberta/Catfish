using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Catfish.Core.Models;
using Catfish.Core.Services;
using ElmahCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Areas.Manager.Pages
{
    public class BackupEntry
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class SchemaPageModel : PageModel
    {
        public Guid Id { get; set; }
        public Guid ActiveBackupId { get; set; }

        [BindProperty]
        [DataType(DataType.MultilineText)]
        public string SchemaXml { get; set; }
        public string SchemaName { get; set; }

        public string ErrorMessage { get; set; } = null;
        public string SuccessMessage { get; set; } = null;

        public List<BackupEntry> Backups { get; set; }

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

        private List<BackupEntry> GetBackups(Guid srcId)
        {
            return _db.Backups
                .Where(bk => bk.SourceId == srcId)
                .OrderByDescending(bk => bk.Timestamp)
                .Select(bk => new BackupEntry() { Id = bk.Id, Timestamp = bk.Timestamp })
                .ToList();
        }

        public void OnGet(Guid id, string successMessage, Guid? backupId)
        {
            Id = id;
            if (backupId.HasValue)
            {
                ActiveBackupId = backupId.Value;
                Backup backup = _db.Backups.Where(bk => bk.Id == backupId).FirstOrDefault();
                if (backup != null && backup.SourceId == id)
                {
                    SchemaXml = backup.SourceData;
                    SchemaName = _db.EntityTemplates.Where(en => en.Id == id).Select(en => en.TemplateName).FirstOrDefault();
                    if(SchemaName == null)
                    {
                        Entity entity = _db.Entities.Where(en => en.Id == id).FirstOrDefault();
                        if (entity != null)
                            SchemaName = entity.ConcatenatedName;
                    }
                }
                else
                {
                    SchemaXml = "";
                    ErrorMessage = "No back-up found";
                }

                Backups = GetBackups(id);
            }
            else
            {
                Entity entity = _db.Entities.Where(et => et.Id == id).FirstOrDefault();

                if (entity != null)
                {
                    SchemaXml = entity.Content;
                    Backups = GetBackups(id);
                    SuccessMessage = successMessage;
                    SchemaName = entity.ConcatenatedName;
                }
                else
                    SchemaXml = "";
            }
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
                bool changed = false;
                if (entity == null)
                {
                    string typeString = xml.Attribute("model-type").Value;
                    var type = Type.GetType(typeString);
                    entity = Entity.Parse(xml, false) as Entity;
                    _db.Entities.Add(entity);
                    id = entity.Id;
                    changed = true;

                }
                else if(Regex.Replace(entity.Content, @"\s+", "") != Regex.Replace(SchemaXml, @"\s+", ""))
                {
                    var user = _authorizationService.GetLoggedUser();
                    Backup backup = new Backup(entity.Id,
                        entity.GetType().ToString(),
                        entity.Content,
                        user.Id,
                        user.UserName);
                    _db.Backups.Add(backup);
                    
                    var dbEntityId = entity.Id;

                    entity.Content = SchemaXml;
                    entity.Updated = DateTime.Now;

                    //restoring the ID
                    entity.Id = dbEntityId;

                    changed = true;
                }

                if (changed)
                {
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
                                var dbState = _workflowService.GetStatus(template.Id, state.Value, false);
                                if(dbState == null)
                                {
                                    if (_db.SystemStatuses.Where(st => st.Id == state.Id).Any())
                                        throw new Exception(string.Format("Error: the System Status with ID {0} already exist associated with another template in the system.", state.Id));

                                    //Creating a new status with the same GUID
                                    dbState = new SystemStatus() 
                                    {
                                        Status = state.Value, 
                                        NormalizedStatus = state.Value.ToUpper(),
                                        Id = state.Id,
                                        EntityTemplateId = template.Id
                                    };
                                    _db.SystemStatuses.Add(dbState);
                                }
                                else if (state.Id != dbState.Id)
                                {
                                    oldGuids.Add(state.Id.ToString());
                                    newGuids.Add(dbState.Id.ToString());
                                }
                            }

                            //Making sure the roles defined in the workflow matches with roles stored in 
                            //the database (and creating new roles in the database if matching ones are not available.
                            foreach (var role in template.Workflow.Roles)
                            {
                                var dbRole = _authorizationService.GetRole(role.Value, false);
                                if(dbRole == null)
                                {
                                    //Creating a new role with the given name and the Guid.
                                    _authorizationService.CreateRole(role.Value, role.Id);
                                }
                                else if (role.Id != dbRole.Id)
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
                }
                else
                    successMessage = "Nothing to save. Schema wasn't changed.";

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

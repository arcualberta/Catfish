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
            string message;
            if (_workflowService.UpdateItemTemplateSchema(id, SchemaXml, out message))
                return RedirectToPage(new { id, message });
            else
            {
                _errorLog.Log(new Error(new Exception(message)));
                ErrorMessage = message;
                return null;
            }
        }

    }
}

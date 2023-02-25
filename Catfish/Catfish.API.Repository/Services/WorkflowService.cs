using Catfish.API.Authorization.Interfaces;
using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Models.Forms;
using Catfish.API.Repository.Models.Workflow;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Claims;

namespace Catfish.API.Repository.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly RepoDbContext _context;
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public WorkflowService(RepoDbContext context, IEntityTemplateService entityTemplateService, IUserService userService, IEmailService emailService)
        {
            _context = context;
            _entityTemplateService = entityTemplateService;
            _userService = userService;
            _emailService = emailService;
        }

        public async Task<WorkflowDbRecord?> GetWorkflowDbRecord(Guid id)
        {
            return await _context.Workflows.Where(w => w.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Workflow?> GetWorkFlow(Guid id)
        {
            WorkflowDbRecord workflowRecord = await _context.Workflows.Where(w => w.Id == id).FirstOrDefaultAsync();

            if (workflowRecord == null)
                return null;

            Workflow wrkFlow = workflowRecord.Workflow;

            return wrkFlow;

        }

        public async Task<List<Workflow>> GetWorkflows()
        {
            List<WorkflowDbRecord> workflowRecords = await _context.Workflows.ToListAsync();
            return workflowRecords.Select(wr => wr.Workflow).ToList();
        }

        public Workflow GetWorkFlowDetails(Guid id)
        {
            WorkflowDbRecord workflowRecord =  _context.Workflows.Where(w => w.Id == id).FirstOrDefault();

            if (workflowRecord == null)
                return null;

            Workflow wrkFlow = workflowRecord.Workflow;

            return wrkFlow;
        }
        
        public bool ExecuteTriggers(Guid workflowId, Guid actionId, Guid buttonId)
        {
            try
            {
                Workflow workflow = GetWorkFlowDetails(workflowId);
                // get entity template using workflow
                EntityTemplate template = _entityTemplateService.GetEntityTemplate(workflow.EntityTemplateId);

                // get list trigger referances of given action 
                WorkflowAction action = workflow.Actions.Where(a => a.Id == actionId).FirstOrDefault();
                Button button = action?.Buttons.Where(b => b.Id == buttonId).FirstOrDefault();

                foreach (var triggerId in button!.Triggers)
                {
                    WorkflowTrigger trigger = workflow.Triggers.Where(tr => tr.Id == triggerId).FirstOrDefault();
                    if (trigger?.Type == eTriggerType.Email)
                    {
                        List<Email> emails = new List<Email>();
                        Email email = new Email();

                        WorkflowEmailTemplate emailTemplate = workflow.EmailTemplates.Where(a => a.Id == trigger.TemplateId).FirstOrDefault();
                        email.Subject = emailTemplate.EmailSubject;
                        email.Body = emailTemplate.EmailBody;
                        foreach (var recipient in trigger.Recipients)
                        {
                            List<string> emailAddtress = new List<string>();
                            switch (recipient.RecipientType)
                            {
                                case eRecipientType.Role:
                                    emailAddtress.AddRange(GetRoleDetails(recipient));
                                    break;
                                case eRecipientType.Owner:
                                    emailAddtress.Add(GetLoggedUserEmail());
                                    break;
                                case eRecipientType.Email:
                                    emailAddtress.Add(recipient.Email);
                                    break;
                                case eRecipientType.FormField:
                                    emails.Add(GetFormFieldDetails(email, recipient));
                                    break;
                                case eRecipientType.MetadataField:
                                    emails.Add(GetMetadataFieldDetails(email, recipient));
                                    break;
                                default:
                                    // code block
                                    break;
                            }
                            switch (recipient.EmailType)
                            {
                                case eEmailType.To:
                                    email.ToRecipientEmail.AddRange(emailAddtress);
                                    break;
                                case eEmailType.Cc:
                                    email.CcRecipientEmail.AddRange(emailAddtress);
                                    break;
                                case eEmailType.Bcc:
                                    email.BccRecipientEmail.AddRange(emailAddtress);
                                    break;
                                    emails.Add(GetMetadataFieldDetails(email, recipient));
                                    break;
                                default:
                                    // code block
                                    break;
                            }

                        }
                        _emailService.SendEmail(email);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private string GetLoggedUserEmail()
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string email = _userService.GetUserById(Guid.Parse(userId)).Email;
            return email;
        }

        private List<string> GetRoleDetails(Recipient recipient)
        {
            List<string> emails = new List<string>();
            foreach(var userId in recipient.Users)
            {
                string? email = _userService.GetUserById(userId).Email;
                if(email != null)
                {
                    emails.Add(email);
                }
            }
            return emails;
        }
        
        
        private Email GetFormFieldDetails(Email email, Recipient recipient)
        {
            return email;
        }
        private Email GetMetadataFieldDetails(Email email, Recipient recipient)
        {
            return email;
        }
    }
}

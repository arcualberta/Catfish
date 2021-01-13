using Catfish.Core.Models;
using ElmahCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NRules.Fluent;
using NRules;

namespace Catfish.Core.Services.Automation
{
    public class RulesService : DbEntityService
    {
        public RulesService(AppDbContext db, ErrorLog errorLog)
        : base(db, errorLog)
        {
        }

        public bool ProcessTriggers(Guid itemId)
        {
            try
            {
                //Loading the item from the databse
                Item item = Db.Items.Where(it => it.Id == itemId).FirstOrDefault();
                if (item == null)
                    throw new Exception(string.Format("Item with ID {0} not found", itemId));

                if (!item.TemplateId.HasValue)
                    throw new Exception(string.Format("The item with ID {0} has not template-id associated with it", itemId));

                //Creating the rules engine session
                ISession rulesEngineSession = CreateRuleEngineSession(item.TemplateId.Value);

                //Getting the audit entries associated with the item
                var auditEntries = item.AuditTrail;


                return true;
            }
            catch(Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return false;
            }
        }

        public ISession CreateRuleEngineSession(Guid entityTemplateId)
        {
            //Reference: https://github.com/NRules/NRules/wiki/Getting-Started

            EntityTemplate template = Db.EntityTemplates.Where(et => et.Id == entityTemplateId).FirstOrDefault();
            if (template == null)
                throw new Exception(string.Format("No entity template with ID {0} found.", entityTemplateId));

            //Load rules
            var repository = new RuleRepository();
            repository.Load(x => x.From(typeof(RulesService).Assembly));

            //Compile rules
            var factory = repository.Compile();

            //Create a working session
            var session = factory.CreateSession();

            return session;

        }
    }
}

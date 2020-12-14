using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.Entity;
using ElmahCore;
using Catfish.Core.Models.Contents;

namespace Catfish.Core.Services
{
    public interface IJobService
    {
        public bool ProcessTriggers(Guid entityId);
    }

    public class JobService : IJobService
    {
        public readonly AppDbContext _db;
        public readonly ErrorLog _errorLog;
        public readonly IServiceProvider _serviceProvider;
        public JobService(AppDbContext db, ErrorLog errorLog, IServiceProvider serviceProvider)
        {
            _db = db;
            _errorLog = errorLog;
            _serviceProvider = serviceProvider;
        }
        public bool ProcessTriggers(Guid entityId)
        {
            try
            {
                Entity entity = _db.Entities.Include(e => e.Template).Where(e => e.Id == entityId).FirstOrDefault();
                if (entity == null)
                    throw new Exception(string.Format("{Entity specified by ID {0} not found.", entityId));

                if (typeof(EntityTemplate).IsAssignableFrom(entity.GetType()))
                {
                    //What is identified by the input entityId is really an entity template
                    ProcessTemplateTriggers(entity as EntityTemplate);
                }
                else
                {
                    ProcessInstanceTriggers(entity);
                }

                return true;
            }
            catch(Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return false;
            }
        }

        protected bool ProcessInstanceTriggers(Entity entity)
        {
            //Taking the latest audit entry of the entity
            AuditEntry auditEntry = entity.AuditTrail.OrderBy(a => a.Created).LastOrDefault();
            if (auditEntry != null)
            {
                //Get the post action that coused this state change

            }

            return true;
        }

        protected bool ProcessTemplateTriggers(EntityTemplate template)
        {

            return true;
        }
    }
}

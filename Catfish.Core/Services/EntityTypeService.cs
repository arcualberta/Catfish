using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using ElmahCore;

namespace Catfish.Core.Services
{
    public class EntityTypeService
    {
        protected AppDbContext Db;
        private readonly ErrorLog _errorLog;
        public EntityTypeService(AppDbContext db, ErrorLog errorLog)
        {
            Db = db;
            _errorLog = errorLog;
        }

        public IQueryable<EntityTemplate> GetEntityTemplates()
        {
            try
            {
                return Db.EntityTemplates.Select(et => et);
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        public List<EntityTemplateListEntry> GetEntityTemplateListEntries()
        {
            try
            {
                return Db.EntityTemplates
                                .Select(et => new EntityTemplateListEntry(et))
                                .ToList()
                                .OrderBy(et => et.ModelType)
                                .ToList();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }
    }
}

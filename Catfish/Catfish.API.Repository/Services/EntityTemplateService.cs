using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Models.Forms;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Catfish.API.Repository.Services
{
    public class EntityTemplateService : IEntityTemplateService
    {
        private readonly RepoDbContext _context;

        public EntityTemplateService(RepoDbContext context)
        {
            _context = context;
        }
        public EntityTemplate GetEntityTemplate(Guid id)
        {
            return _context.EntityTemplates!.Where(t => t.Id == id).FirstOrDefault();
        }

        public async Task<HttpStatusCode> AddEntity(EntityTemplate entityTemplate)
        {
            List<FormTemplate> associatedForms = await LoadAssociatedForms(entityTemplate);
            foreach(var form in associatedForms)
                entityTemplate.Forms.Add(form);

            _context.EntityTemplates!.Add(entityTemplate);
            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> UpdateEntity(EntityTemplate entityTemplate)
        {
            //Loading the entity from the database
            EntityTemplate? dbEntityTemplate = await _context.EntityTemplates!.Include(et => et.Forms).FirstOrDefaultAsync(et => et.Id == entityTemplate.Id);
            if (dbEntityTemplate == null)
                return HttpStatusCode.NotFound;

            //Loading the list of forms to be associated with the dbEntityTemplate
            List<FormTemplate> associatedForms = await LoadAssociatedForms(entityTemplate);

            //Add new form associations that do not already exist in the database
            foreach (FormTemplate form in associatedForms)
                if (!dbEntityTemplate.Forms.Any(f => f.Id == form.Id))
                    dbEntityTemplate.Forms.Add(form);

            //Remove forms that are no longer in the forms associated with the input entityTemplate
            var keepingFormIds = associatedForms.Select(form => form.Id);
            var toBeRemoved = dbEntityTemplate.Forms.Where(f => !keepingFormIds.Contains(f.Id)).ToList();
            foreach (var form in toBeRemoved)
                dbEntityTemplate.Forms.Remove(form);

            //Copying property values to the dbEntityTemplate
            dbEntityTemplate.Description = entityTemplate.Description;
            dbEntityTemplate.Updated = DateTime.Now;
            dbEntityTemplate.Name = entityTemplate.Name;
            dbEntityTemplate.EntityTemplateSettings = entityTemplate.EntityTemplateSettings;

            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> ChangeStatus(EntityTemplate entityTemplate, eState state)
        {
            //Loading the entity from the database
            EntityTemplate? dbEntityTemplate = await _context.EntityTemplates!.Include(et => et.Forms).FirstOrDefaultAsync(et => et.Id == entityTemplate.Id);
            if (dbEntityTemplate == null)
                return HttpStatusCode.NotFound;

            dbEntityTemplate.State = state;
            dbEntityTemplate.Updated = DateTime.Now;
           

            return HttpStatusCode.OK;
        }


        #region Private Methods

        private async Task<List<FormTemplate>> LoadAssociatedForms(EntityTemplate entityTemplate)
        {
            var formIds = entityTemplate.EntityTemplateSettings?.DataForms.Select(form => form.Id)
                .Union(entityTemplate.EntityTemplateSettings.MetadataForms.Select(form => form.Id))
                .ToList();

            if (formIds != null && formIds.Any())
                return await _context.Forms!.Where(form => formIds!.Contains(form.Id)).ToListAsync();
            else
                return new List<FormTemplate>();
        }
        #endregion
    }
}

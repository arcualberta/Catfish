using Catfish.API.Repository.Interfaces;

namespace Catfish.API.Repository.Services
{
    public class EntityTemplateService : IEntityTemplateService
    {
        private readonly RepoDbContext _context;

        public EntityTemplateService(RepoDbContext context)
        {
            _context = context;
        }
        public void UpdateEntityTemplateSettings(EntityTemplate entityTemplate)
        {
            entityTemplate.Forms.Clear();
            if (entityTemplate.EntityTemplateSettings == null)
                throw new CatfishException("Entity template settings is null in the template");

            foreach (FormEntry fe in entityTemplate.EntityTemplateSettings.MetadataForms)
            {
                Form? frm = _context.Forms!.SingleOrDefault(f => f.Id == fe.FormId);
                if (frm == null)
                    throw new CatfishException($"No form with ID {fe.FormId} found");

                entityTemplate.Forms.Add(frm);
            }

            foreach (FormEntry fe in entityTemplate.EntityTemplateSettings.DataForms)
            {
                Form? frm = _context.Forms!.SingleOrDefault(f => f.Id == fe.FormId);
                if (frm == null)
                    throw new CatfishException($"No form with ID {fe.FormId} found");

                entityTemplate.Forms.Add(frm);
            }
        }
    }
}

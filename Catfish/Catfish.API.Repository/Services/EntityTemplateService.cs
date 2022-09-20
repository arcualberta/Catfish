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
        public void SaveEntityTemplate(EntityTemplate entityTemplate)
        {
            //Clear the Forms collection
            //Go over each entry in the MetadataForms and DataForms arrays
            //Grab the form for each entry
            //Add them into the Forms collection,
            try {
                entityTemplate.Forms.Clear();
                if (entityTemplate.EntityTemplateSettings != null)
                {
                    foreach (FormEntry fe in entityTemplate.EntityTemplateSettings.MetadataForms)
                    {
                        if (!(fe.FormId == Guid.Empty))
                        {
                            Form? frm = _context.Forms?.SingleOrDefault(f => f.Id == fe.FormId);
                            if (frm != null)
                                entityTemplate.Forms.Add(frm);
                        }
                    }
                    foreach (FormEntry fe in entityTemplate.EntityTemplateSettings.DataForms)
                    {
                        if (!(fe.FormId == Guid.Empty))
                        {
                            Form? frm = _context.Forms?.SingleOrDefault(f => f.Id == fe.FormId);
                            if (frm != null)
                                entityTemplate.Forms.Add(frm);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
         }
    }
}

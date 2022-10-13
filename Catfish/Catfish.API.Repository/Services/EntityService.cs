using Catfish.API.Repository.Interfaces;
using Catfish.API.Repository.Models.Forms;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Catfish.API.Repository.Services
{
    public class EntityService : IEntityService
    {
        private readonly RepoDbContext _context;

        public EntityService(RepoDbContext context)
        {
            _context = context;
        }
        public Entity GetEntity(Guid id)
        {
            return _context.Entities.Where(t => t.Id == id).FirstOrDefault();
        }

        public async Task<HttpStatusCode> AddEntity(Entity entity, List<IFormFile> files, List<string> fileKeys)
        {
            if (files.Count > 0 && fileKeys.Count > 0)
            {
                AttachFiles(files, fileKeys, entity);
            }
            _context.Entities!.Add(entity);
            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> UpdateEntity(Entity entity)
        {
            throw new NotImplementedException();
        }
        protected void AttachFiles(List<IFormFile> files, List<string> fileKeys, Entity dst)
        {
            //Grouping files by attachment field IDs into a dictionary
            Dictionary<Guid, List<IFormFile>> groupdFileList = new Dictionary<Guid, List<IFormFile>>();
            for (int i = 0; i < Math.Min(files.Count, fileKeys.Count); ++i)
            {
                Guid attachmentId = Guid.Parse(fileKeys[i]);
                if (!groupdFileList.ContainsKey(attachmentId))
                    groupdFileList.Add(attachmentId, new List<IFormFile>());

                groupdFileList[attachmentId].Add(files[i]);
            }

            string uploadRoot = GetAttachmentsFolder(true);
            foreach (var key in groupdFileList.Keys)
            {
               // List<FileReference> fileReferences = _itemService.UploadFiles(groupdFileList[key], uploadRoot);
               // AttachmentField field = dst.Fields.First(f => f.Id == key) as AttachmentField;
               // foreach (FileReference fileReference in fileReferences)
                //    field.Files.Add(fileReference);
            }
        }
        private static string GetAttachmentsFolder(bool createIfNotExist = false)
        {
            //ConfigurationManager configuration = new ConfigurationManager();
           
           // string path = Path.Combine(configuration.GetSection("SiteConfig:UploadRoot").Value, "attachments"); ;
            string path = Path.Combine("App_Data/uploads/", "attachments"); ;

            if (createIfNotExist)
                Directory.CreateDirectory(path);

            return path;
        }

    }
}

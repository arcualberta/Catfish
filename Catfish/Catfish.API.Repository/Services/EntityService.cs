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
        public EntityData GetEntity(Guid id)
        {
            return _context.Entities.Where(t => t.Id == id).FirstOrDefault();
        }

        public List<EntityEntry> GetEntities(eEntityType entityType, eSearchTarget searchTarget, string searchText, int offset = 0, int? max = null)
        {
            var query = _context.Entities.Where(e => e.EntityType == entityType);
            if(searchTarget == eSearchTarget.Title)
            {
                query.Where(e=>e.Title.Contains( searchText));
            }
            else if(searchTarget == eSearchTarget.Description)
            {
                query.Where(e=>e.Description == searchText);
            }
            else
            {
                query.Where(e => e.Title == searchText || e.Description == searchText);
            }

            if (offset > 0)
                query.Skip(offset);
            if (max != null && max > 0)
                query.Take(max.Value);

            return query.Select(e => new EntityEntry { Id = e.Id, Title = e.Title, Description = e.Description, Created = e.Created, Updated = e.Updated }).ToList();
        }
            public async Task<HttpStatusCode> AddEntity(EntityData entity, List<IFormFile> files, List<string> fileKeys)
        {
            if (files.Count > 0 && fileKeys.Count > 0)
            {
                AttachFiles(files, fileKeys, entity);
            }
            _context.Entities!.Add(entity);
            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> UpdateEntity(EntityData entity)
        {
            throw new NotImplementedException();
        }
        protected void AttachFiles(List<IFormFile> files, List<string> fileKeys, EntityData dst)
        {
            //Grouping files by attachment field IDs into a dictionary
            //Dictionary<Guid, List<IFormFile>> groupdFileList = new Dictionary<Guid, List<IFormFile>>();
            //for (int i = 0; i < Math.Min(files.Count, fileKeys.Count); ++i)
            //{
            //    Guid attachmentId = Guid.Parse(fileKeys[i]);
            //    if (!groupdFileList.ContainsKey(attachmentId))
            //        groupdFileList.Add(attachmentId, new List<IFormFile>());

            //    groupdFileList[attachmentId].Add(files[i]);
            //}

            string uploadRoot = GetAttachmentsFolder(true);
            // foreach (var key in groupdFileList.Keys)
            // {
            // List<FileReference> fileReferences = _itemService.UploadFiles(groupdFileList[key], uploadRoot);
            // AttachmentField field = dst.Fields.First(f => f.Id == key) as AttachmentField;
            // foreach (FileReference fileReference in fileReferences)
            //    field.Files.Add(fileReference);
            // }
            int fileIdx = 0;
           foreach(string fk in fileKeys)
            {
                UploadFile(files.ElementAt(fileIdx), fk, uploadRoot);
                fileIdx++;
            }
        }

        public void UploadFile(IFormFile file, string fileKey, string uploadRoot)
        {
                //Destination absolute path name
                string pathName = Path.Combine(uploadRoot,fileKey+"_"+ file.FileName);
                using (var stream = new FileStream(pathName, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
        }

        private static string GetAttachmentsFolder(bool createIfNotExist = false)
        {
            //ConfigurationManager configuration = new ConfigurationManager();
           
           // string path = Path.Combine(configuration.GetSection("SiteConfig:UploadRoot").Value, "attachments"); ;
            string path = Path.Combine("App_Data/uploads/", "attachments"); ;

            if (createIfNotExist)
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

    }
}

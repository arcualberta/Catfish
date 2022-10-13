using System.Net;

namespace Catfish.API.Repository.Interfaces
{
    public interface IEntityService
    {
        public Task<HttpStatusCode> AddEntity(EntityData entityTemplate, List<IFormFile> files, List<string> fileKeys);
        public Task<HttpStatusCode> UpdateEntity(EntityData entityTemplate);
        public EntityData GetEntity(Guid id);
    }
}

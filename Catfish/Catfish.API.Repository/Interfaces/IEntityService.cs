using System.Net;

namespace Catfish.API.Repository.Interfaces
{
    public interface IEntityService
    {
        public Task<HttpStatusCode> AddEntity(Entity entityTemplate, List<IFormFile> files, List<string> fileKeys);
        public Task<HttpStatusCode> UpdateEntity(Entity entityTemplate);
        public Entity GetEntity(Guid id);
    }
}

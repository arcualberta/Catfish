using System.Net;

namespace Catfish.API.Repository.Interfaces
{
    public interface IEntityTemplateService
    {
        public Task<HttpStatusCode> AddEntity(EntityTemplate entityTemplate);
        public Task<HttpStatusCode> UpdateEntity(EntityTemplate entityTemplate);
    }
}

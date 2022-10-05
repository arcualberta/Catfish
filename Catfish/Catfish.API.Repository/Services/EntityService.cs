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

        public async Task<HttpStatusCode> AddEntity(Entity entity)
        {
            _context.Entities!.Add(entity);
            return HttpStatusCode.OK;
        }

        public async Task<HttpStatusCode> UpdateEntity(Entity entity)
        {
            throw new NotImplementedException();
        }

       

    }
}

using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly IAuthorizationService _authorizationService;
        protected IEmailService _emailService;
        private readonly AppDbContext _db;
        public  SubmissionService(IAuthorizationService auth, IEmailService email, AppDbContext db)
        {
            _authorizationService = auth;
            _emailService = email;
            _db = db;
        }
        public List<Entity> GetSubmissionList()
        {
            List<Entity> authorizeList = new List<Entity>();
            return authorizeList;
        }
        public Entity GetSubmissionDetails(Guid id)
        {
            Entity submissionData = new Entity();
            return submissionData;
        }
        public string SaveSubmission(Entity submission)
        {
            string statusMessege="Submission Saved Sucessfully";
            return statusMessege;
        }
    }
}

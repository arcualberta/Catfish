using Catfish.Core.Models;
using Microsoft.AspNetCore.Identity;
using Piranha.AspNetCore.Identity.Data;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public  SubmissionService(IAuthorizationService auth, IEmailService email, AppDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _authorizationService = auth;
            _emailService = email;
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Get the list of entities created from a give template and with the 
        /// state identified by the given stateGuid. The returned list is limited 
        /// to the entites that are accessible to the curretly logged in user. 
        /// </summary>
        /// <returns></returns>
        public List<Entity> GetEntityList(Guid? templateId, Guid? stateGuid)
        {
            //var ev = _db.Entities.Where(e => e.TemplateId == templateId && StateId == stateGuid ?).FirstOrDefault();
            List<Entity> authorizeList = new List<Entity>();
            return authorizeList;
        }

        /// <summary>
        /// Get the submission details which passing from the parameter.  
        /// </summary>
        /// <returns></returns>
        public Entity GetSubmissionDetails(Guid id)
        {
            Entity ev = _db.Entities.Where(e => e.Id == id).FirstOrDefault();
            return ev;
        }

        /// <summary>
        /// Save submission details which passing from the parameter.   
        /// </summary>
        /// <returns></returns>
        public string SaveSubmission(Entity submission)
        {
            string statusMessege="Submission Saved Sucessfully";
            return statusMessege;
        }
    }
}

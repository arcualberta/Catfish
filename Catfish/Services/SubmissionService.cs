using Catfish.Core.Models;
using ElmahCore;
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
        private readonly ErrorLog _errorLog;
        public SubmissionService(IAuthorizationService auth, IEmailService email, AppDbContext db, ErrorLog errorLog)
        {
            _authorizationService = auth;
            _emailService = email;
            _db = db;
            _errorLog = errorLog;
        }

        ///// <summary>
        ///// Get the list of entities created from a give template and with the 
        ///// state identified by the given stateGuid. The returned list is limited 
        ///// to the entites that are accessible to the curretly logged in user. 
        ///// </summary>
        ///// <returns></returns>
        //public List<Entity> GetEntityList(Guid? templateId, Guid? stateGuid)
        //{
        //    //var ev = _db.Entities.Where(e => e.TemplateId == templateId && StateId == stateGuid ?).FirstOrDefault();
        //    List<Entity> authorizeList = new List<Entity>();
        //    return authorizeList;
        //}

        ///// <summary>
        ///// Get the submission details which passing from the parameter.  
        ///// </summary>
        ///// <returns></returns>
        //public Entity GetSubmissionDetails(Guid id)
        //{
        //    Entity ev = _db.Entities.Where(e => e.Id == id).FirstOrDefault();
        //    return ev;
        //}

        ///// <summary>
        ///// Save submission details which passing from the parameter.   
        ///// </summary>
        ///// <returns></returns>
        //public string SaveSubmission(Entity submission)
        //{
        //    string statusMessege="Submission Saved Sucessfully";
        //    return statusMessege;
        //}
        public List<Item> GetSubmissionList()
        {
            List<Item> items = new List<Item>();
            try
            {
                items = _db.Items.ToList();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
            return items;

        }

        ///// <summary>
        ///// Get the submission details which passing from the parameter.  
        ///// </summary>
        ///// <returns></returns>
        public Item GetSubmissionDetails(Guid itemId)
        {
            Item itemDetails = new Item();
            try
            {
                itemDetails = _db.Items.Where(it => it.Id == itemId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
            return itemDetails;
        }

        /// <summary>
        /// Get the submission list which passing the parameters
        /// if the collection id is null then it just return items which are belongs to the template id
        /// otherwise that should return items which are belongs to the template id and collection id
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        public IList<Item> GetSubmissionList(Guid templateId, Guid? collectionId)
        {
            IList<Item> itemList = new List<Item>();
            try
            {
                var query = _db.Items.Where(i => i.TemplateId == templateId);

                if (collectionId != null)
                    query = query.Where(i => i.PrimaryCollectionId == collectionId);

                itemList = query.ToList();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
            }
            return itemList;
        }
    }
}

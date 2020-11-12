using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public interface ISubmissionService
    {
        //List<Entity> GetEntityList(Guid? templateId, Guid? stateGuid);
        //Entity GetSubmissionDetails(Guid id);
        //string SaveSubmission(Entity submission);
        List<Item> GetSubmissionList();
        Item GetSubmissionDetails(Guid itemId);
        IList<Item> GetSubmissionList(Guid templateId, Guid? collectionId);
    }
}

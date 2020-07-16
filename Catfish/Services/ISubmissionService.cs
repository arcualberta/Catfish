using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public interface ISubmissionService
    {
        List<Entity> GetSubmissionList();
        Entity GetSubmissionDetails(Guid id);
        string SaveSubmission(Entity submission);
    }
}

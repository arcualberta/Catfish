using Catfish.Core.Models;
using Catfish.Core.Models.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public class AccessDefinitionService : EntityService
    {
        public AccessDefinitionService(CatfishDbContext db) : base(db) { }

        public CFAccessDefinition EditAccessDefinition(CFAccessDefinition accessDefinition)
        {
            if(accessDefinition.Id > 0)
            {
                //edit
                Db.Entry(accessDefinition).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                //add new
                Db.AccessDefinitions.Add(accessDefinition);
            }

            return accessDefinition;
        }

        public IQueryable<CFAccessDefinition> GetAccessDefinitions()
        {
            return Db.AccessDefinitions;
        }

        public CFAccessDefinition GetAccessDefinitionById(int id)
        {
            return Db.AccessDefinitions.Where(a => a.Id == id).FirstOrDefault();
        }

        public void DeleteAccessDefinition(int id)
        {
            CFAccessDefinition model = null;
            if (id > 0)
            {
                model = Db.AccessDefinitions.Where(et => et.Id == id).FirstOrDefault();
                if (model != null)
                {
                    Db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                }
                else
                {
                    throw new ArgumentException(string.Format("Access Definitions {0} not found.", id));
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Invalid Access Definitions id {0}.", id));
            }
        }
    }
}

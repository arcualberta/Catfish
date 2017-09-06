using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Catfish.Core.Services
{
    public class CollectionService: EntityService
    {
        public CollectionService(CatfishDbContext db) : base(db) { }

        public IEnumerable<Entity> GetChildItems(int collectionId, int offset = 0, int max = Int32.MaxValue)
        {
            Collection collection = Db.Collections.Include(c => c.ChildItems).Where(c => c.Id == collectionId).FirstOrDefault();
            if (collection == null)
                return new List<Entity>();

            return collection.ChildItems.Skip(offset).Take(max);
        }
    }
}

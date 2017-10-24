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

        public Collection UpdateStoredCollection(Collection changedCollection)
        {
            Collection dbModel = new Collection();

            if (changedCollection.Id > 0)
            {
                dbModel = Db.XmlModels.Find(changedCollection.Id) as Collection;
                dbModel.Deserialize();

                //updating the "value" text elements
                dbModel.UpdateValues(changedCollection);
            }
            else
            {
                dbModel = CreateEntity<Collection>(changedCollection.EntityTypeId.Value);
                dbModel.Deserialize();
                dbModel.UpdateValues(changedCollection);
            }

            //Serializing the XML model into the Content field.
            dbModel.Serialize();

            if (changedCollection.Id > 0) //update Item
                Db.Entry(dbModel).State = EntityState.Modified;
            else
                Db.XmlModels.Add(dbModel);

            return dbModel;
        }
    }
}

using Catfish.Core.Models;
using System.Data.Entity;
using System.Linq;

namespace Catfish.Core.Services
{
    public class CollectionService: EntityService
    {
        public CollectionService(CatfishDbContext db) : base(db) { }

        public Collection GetCollection(int id)
        {
            return Db.Collections.Find(id);
        }

        public Collection GetCollectionByGuid(string guid)
        {
            return Db.Collections.Where(c => c.MappedGuid == guid).FirstOrDefault();
        }

        public Collection CreateCollection(int entityTypeId)
        {
            return CreateEntity<Collection>(entityTypeId);
        }

        public Collection UpdateStoredCollection(Collection changedCollection)
        {
            Collection dbModel = new Collection();

            if (changedCollection.Id > 0)
            {
                dbModel = GetCollection(changedCollection.Id);

                //updating the "value" text elements
                dbModel.UpdateValues(changedCollection);
            }
            else
            {
                dbModel = CreateEntity<Collection>(changedCollection.EntityTypeId.Value);
                dbModel.UpdateValues(changedCollection);
            }

            dbModel.Serialize();
            if (changedCollection.Id > 0) //update Item
                Db.Entry(dbModel).State = EntityState.Modified;
            else
            {
                Db.XmlModels.Add(dbModel);
            }

            return dbModel;
        }

    }
}

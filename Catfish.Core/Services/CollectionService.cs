using System;
using Catfish.Core.Models;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;

namespace Catfish.Core.Services
{
    /// <summary>
    /// A Service used to perform actions on Collection Entitites.
    /// </summary>
    public class CollectionService: EntityService
    {
        /// <summary>
        /// Create an instance of the CollectionService.
        /// </summary>
        /// <param name="db">The database context containing the needed Collections.</param>
        public CollectionService(CatfishDbContext db) : base(db) { }

        /// <summary>
        /// Get a collection from the database.
        /// </summary>
        /// <param name="id">The id of the Collection to obtain.</param>
        /// <returns>The requested collection from the database. A null value is returned if no collection is found.</returns>
        public Collection GetCollection(int id)
        {
            return Db.Collections.Where(c => c.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get a collection from the database.
        /// </summary>
        /// <param name="guid">The mapped guid of the Collection to obtain.</param>
        /// <returns>The requested collection from the database. A null value is returned if no collection is found.</returns>
        public Collection GetCollection(string guid)
        {
            return Db.Collections.Where(c => c.MappedGuid == guid).FirstOrDefault();
        }

        /// <summary>
        /// Get all collections accessable by the current user.
        /// </summary>
        /// <returns>The resulting list of collections.</returns>
        public IQueryable<Collection> GetCollections()
        {
            return Db.Collections;
        }

        /// <summary>
        /// Removes a collection from the database.
        /// </summary>
        /// <param name="id">The id of the collection to be removed.</param>
        public void DeleteCollection(int id)
        {
            Collection model = null;
            if (id > 0)
            {
                model = Db.Collections.Where(et => et.Id == id).FirstOrDefault();
                if (model != null)
                {
                    Db.Entry(model).State = EntityState.Deleted;
                }
                else
                {
                    throw new ArgumentException(string.Format("Collection {0} not found.", id));
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Invalid collection id {0}.", id));
            }
        }

        /// <summary>
        /// Creates a new collection based on the given entity type.
        /// </summary>
        /// <param name="entityTypeId">The Id of the entity type to connect to the collection.</param>
        /// <returns>The newly created collection.</returns>
        public Collection CreateCollection(int entityTypeId)
        {
            return CreateEntity<Collection>(entityTypeId);
        }

        /// <summary>
        /// Updates a Collection in the database with the the values provided. A new Collection is created if one does not already exist.
        /// </summary>
        /// <param name="changedCollection">The collection content to be modified.</param>
        /// <returns>The modified database collection.</returns>
        public Collection UpdateStoredCollection(Collection changedCollection)
        {
            Collection dbModel = new Collection();

            if (changedCollection.Id > 0)
            {
                dbModel = GetCollection(changedCollection.Id);
            }
            else
            {
                dbModel = CreateEntity<Collection>(changedCollection.EntityTypeId.Value);
            }

            //updating the "value" text elements
            dbModel.UpdateValues(changedCollection);
            dbModel.Serialize();

            if (changedCollection.Id > 0) //update Item
                Db.Entry(dbModel).State = EntityState.Modified;
            else
            {
                dbModel = Db.Collections.Add(dbModel);
            }

            return dbModel;
        }

    }
}

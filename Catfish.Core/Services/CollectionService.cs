using System;
using Catfish.Core.Models;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Collections.Generic;
using Catfish.Core.Helpers;
using Catfish.Core.Contexts;
using Catfish.Core.Models.Access;
using System.Xml.Linq;

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
        //public CollectionService(CatfishDbContext db, Func<string, bool> isAdmin) : base(db) { }

        /// <summary>
        /// Get a collection from the database.
        /// </summary>
        /// <param name="id">The id of the Collection to obtain.</param>
        /// <returns>The requested collection from the database. A null value is returned if no collection is found.</returns>
        public CFCollection GetCollection(int id, AccessMode accessMode = AccessMode.Read)
        {
            return Db.Collections.FindAccessible(AccessContext.current.IsAdmin,
                AccessContext.current.AllGuids,
                accessMode)
                .Where(i => i.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get a collection from the database.
        /// </summary>
        /// <param name="guid">The mapped guid of the Collection to obtain.</param>
        /// <returns>The requested collection from the database. A null value is returned if no collection is found.</returns>
        //public CFCollection GetCollection(string guid)
        //{
        //    return Db.Collections.Where(c => c.MappedGuid == guid).FirstOrDefault();
        //}

        /// <summary>
        /// Get all collections accessable by the current user.
        /// </summary>
        /// <returns>The resulting list of collections.</returns>
        public IQueryable<CFCollection> GetCollections()
        {
            //string guidString = actor.Name;
            //List<Guid> guids = GetUserGuids(guidString);
            //SecurityServiceBase.CreateSecurityAccessContext(actor.Name);
            
            return Db.Collections.FindAccessible(AccessContext.current.IsAdmin,
                AccessContext.current.AllGuids, 
                Models.Access.AccessMode.Read);
        }

        public List<CFCollection> GetCollections(IEnumerable<int> ids, AccessMode accessMode = AccessMode.Read)
        {
            List<CFCollection> result = new List<CFCollection>();

            foreach (int id in ids)
            {
                result.Add(GetCollection(id, accessMode));
            }

            return result;
        }

        public IQueryable<CFCollection> GetSystemCollections()
        {
            List<CFCollection> result = new List<CFCollection>();
            var collections = Db.Collections.ToList();
            foreach(CFCollection col in collections)
            {
                XAttribute isSysColl = col.Data.Attribute("issystemcollection");
                if (isSysColl != null &&  Convert.ToBoolean(isSysColl.Value))
                {
                    result.Add(col);
                }
            }

            return result.AsQueryable();
        }


        /// <summary>
        /// Removes a collection from the database.
        /// </summary>
        /// <param name="id">The id of the collection to be removed.</param>
        public void DeleteCollection(int id)
        {
            CFCollection model = null;
            if (id > 0)
            {
                model = GetCollection(id, AccessMode.Control);
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
        public CFCollection CreateCollection(int entityTypeId)
        {
            return CreateEntity<CFCollection>(entityTypeId);
        }

        /// <summary>
        /// Updates a Collection in the database with the the values provided. A new Collection is created if one does not already exist.
        /// </summary>
        /// <param name="changedCollection">The collection content to be modified.</param>
        /// <returns>The modified database collection.</returns>
        public CFCollection UpdateStoredCollection(CFCollection changedCollection)
        {
            CFCollection dbModel = new CFCollection();

            if (changedCollection.Id > 0)
            {
                dbModel = GetCollection(changedCollection.Id, AccessMode.Write);
            }
            else
            {
                dbModel = CreateEntity<CFCollection>(changedCollection.EntityTypeId.Value);
            }

            //updating the "value" text elements
            dbModel.UpdateValues(changedCollection);

            //update collection attribute -- issystemCollection -- Aug 21 2019
            XAttribute isSysColl = changedCollection.Data.Attribute("issystemcollection");
            if (isSysColl != null)
            {
                dbModel.Data.SetAttributeValue("issystemcollection", isSysColl.Value);
            }

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

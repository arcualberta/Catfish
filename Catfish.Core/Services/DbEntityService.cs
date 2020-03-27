using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Data.Entity;

namespace Catfish.Core.Services
{
    public class DbEntityService
    {
        protected AppDbContext Db;
        public DbEntityService(AppDbContext db)
        {
            Db = db;
        }

        protected IQueryable<Entity> BuildEntityListRetrieveQuery<T>(int offset = 0, int? max = null, Guid? primaryCollectionId = null)
        {
            //var query = Db.Entities.Include(e => e.PrimaryCollection);
            var query = primaryCollectionId.HasValue ?
                Db.Entities.Include(e => e.PrimaryCollection).Where(e => e is T && e.PrimaryCollectionId == primaryCollectionId) :
                Db.Entities.Include(e => e.PrimaryCollection).Where(e => e is T);

            if (offset > 0)
                query = query.Skip(offset);
            if (max.HasValue)
                query.Take(max.Value);

            return query;
        }
        public IQueryable<Item> GetItems(int offset = 0, int? max = null, Guid? primaryCollectionId = null)
        {
            IQueryable<Entity> query = BuildEntityListRetrieveQuery<Item>(offset, max, primaryCollectionId);
            IQueryable<Item> ret = query.Select(e => e as Item);
            return ret;
        }

        public IQueryable<Collection> GetCollections(int offset = 0, int? max = null, Guid? primaryCollectionId = null)
        {
            IQueryable<Entity> query = BuildEntityListRetrieveQuery<Collection>(offset, max, primaryCollectionId);
            IQueryable<Collection> ret = query.Select(e => e as Collection);
            return ret;
        }


        public IQueryable<EntityListEntry> GetEntityList<T>(int offset = 0, int? max = null, Guid? primaryCollectionId = null)
        {
            IQueryable<Entity> entities = BuildEntityListRetrieveQuery<T>(offset, max, primaryCollectionId);
            IQueryable<EntityListEntry> ret = entities.Select(e => new EntityListEntry(e));
            return ret;
        }

        public Relationship CreateRelationship(Entity subject, string predicate, Entity @object)
        {
            Relationship rel = new Relationship()
            {
                Subject = subject,
                SubjectId = subject.Id,
                Objct = @object,
                ObjctId = @object.Id,
                Predicate = predicate
            };

            return rel;
        }

        public Item GetItem(Guid id)
        {
            return Db.Items
                .Where(e => e.Id == id)
                .FirstOrDefault();
        }
    }
}

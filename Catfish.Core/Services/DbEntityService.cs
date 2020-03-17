using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Catfish.Core.Services
{
    public class DbEntityService
    {
        protected AppDbContext Db;
        public DbEntityService(AppDbContext db)
        {
            Db = db;
        }

        public IQueryable<EntityListEntry> GetEntityList<T>(int offset = 0, int? max = null, int? primaryCollectionId = null)
        {
            var query = Db.Entities.Include(e => e.PrimaryCollection);
            query = primaryCollectionId.HasValue ?
                query.Where(e => e is T && e.PrimaryCollectionId == primaryCollectionId) :
                query.Where(e => e is T);

            if (offset > 0)
                query = query.Skip(offset);
            if (max.HasValue)
                query.Take(max.Value);

            IQueryable<EntityListEntry> ret = query.Select(e => new EntityListEntry(e));
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
    }
}

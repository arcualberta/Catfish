using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using ElmahCore;
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
        protected ErrorLog _errorLog;
        public DbEntityService(AppDbContext db, ErrorLog errorLog)
        {
            Db = db;
            _errorLog = errorLog;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="offset"></param>
        /// <param name="max"></param>
        /// <param name="primaryCollectionId"></param>
        /// <returns></returns>
        protected IQueryable<Entity> BuildEntityListRetrieveQuery<T>(int offset = 0, int? max = null, Guid? primaryCollectionId = null)
        {
            try
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
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="max"></param>
        /// <param name="primaryCollectionId"></param>
        /// <returns></returns>
        public IQueryable<Item> GetItems(int offset = 0, int? max = null, Guid? primaryCollectionId = null)
        {
            try
            {
                IQueryable<Entity> query = BuildEntityListRetrieveQuery<Item>(offset, max, primaryCollectionId);
                IQueryable<Item> ret = query.Select(e => e as Item);
                return ret;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="max"></param>
        /// <param name="primaryCollectionId"></param>
        /// <returns></returns>
        public IQueryable<Collection> GetCollections(int offset = 0, int? max = null, Guid? primaryCollectionId = null)
        {
            try
            {
                IQueryable<Entity> query = BuildEntityListRetrieveQuery<Collection>(offset, max, primaryCollectionId);
                IQueryable<Collection> ret = query.Select(e => e as Collection);
                return ret;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="offset"></param>
        /// <param name="max"></param>
        /// <param name="primaryCollectionId"></param>
        /// <returns></returns>
        public IQueryable<EntityListEntry> GetEntityList<T>(int offset = 0, int? max = null, Guid? primaryCollectionId = null)
        {
            try
            {
                IQueryable<Entity> entities = BuildEntityListRetrieveQuery<T>(offset, max, primaryCollectionId);
                IQueryable<EntityListEntry> ret = entities.Select(e => new EntityListEntry(e));

                return ret;
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="predicate"></param>
        /// <param name="object"></param>
        /// <returns></returns>
        public Relationship CreateRelationship(Entity subject, string predicate, Entity @object)
        {
            try
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
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Item GetItem(Guid id)
        {
            try
            {
                return Db.Items
                .Where(e => e.Id == id)
                .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _errorLog.Log(new Error(ex));
                return null;
            }
        }
        /// <summary>
        /// Get Item created with particular template
        /// </summary>
        /// <param name="id"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public Item GetItem(Guid id, Guid? templateId)
        {
            Item item = Db.Entities.Include(e => e.Template).Where(e => e.Id == id && e.TemplateId == templateId.Value && e is Item)
                                    .FirstOrDefault()as Item ;

            return item;
        }

    }
}

using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Catfish.Core.Helpers;

namespace Catfish.Core.Services
{
    public class EntityService: ServiceBase
    {
        public EntityService(CatfishDbContext db) : base(db) { }

        //public IQueryable<EntityType> GetEntityTypes()
        //{
        //    return Db.EntityTypes;
        //}

        //public IQueryable<EntityType> GetEntityTypes(EntityType.eTarget target)
        //{
        //    // return Db.EntityTypes.Where(et => et.TargetType == target);
        //     return Db.EntityTypes.Where(et => et.TargetTypes.Contains(target.ToString())); //Mr Jan 15 2018
         
        //}

        public T CreateEntity<T>(int entityTypeId) where T : Entity, new()
        {
            EntityType et = Db.EntityTypes.Where(t => t.Id == entityTypeId).FirstOrDefault();
            if (et == null)
                throw new Exception("EntityType with ID " + entityTypeId + " not found");

            T entity = new T();
            entity.EntityType = et;
            entity.EntityTypeId = et.Id;
            entity.InitMetadataSet(et.MetadataSets.ToList());
            entity.SetAttribute("entity-type", et.Name);

            //removing audit trail entry that was created when creating the metadata set originally
            foreach(MetadataSet ms in entity.MetadataSets)
            {
                XElement audit = ms.Data.Element("audit");
                if (audit != null)
                    audit.Remove();
            }

            return entity;
        }

        public void CreateEntityType(EntityType entityType)
        {
            Db.EntityTypes.Add(entityType);
            foreach (var m in entityType.MetadataSets)
            {
                if (m.Id < 1)
                    continue;

                Db.MetadataSets.Attach(m);
            }
        }
        public void UpdateEntityType(EntityType entityType)
        {
            CustomComparer<MetadataSet> compare = new CustomComparer<MetadataSet>((x, y) => x.Id == y.Id);
            EntityType dbEntity = Db.EntityTypes.Where(e => e.Id == entityType.Id).FirstOrDefault();
            dbEntity.Name = entityType.Name;
            dbEntity.Description = entityType.Description;

            var deletedMetaData = dbEntity.MetadataSets.Except(entityType.MetadataSets, compare).ToList();
            deletedMetaData.ForEach(md => dbEntity.MetadataSets.Remove(md));

            var addedMetaData = entityType.MetadataSets.Except(dbEntity.MetadataSets, compare).ToList();
            foreach(MetadataSet md in addedMetaData)
            {
                if (md.Id < 1)
                    continue;

                var mdDb = Db.MetadataSets.Attach(md);
                dbEntity.MetadataSets.Add(mdDb);
            }

            Db.Entry(dbEntity).State = System.Data.Entity.EntityState.Modified;
        }

        public IQueryable<Entity> GetEntitiesTextSearch(string searchString, string[] languageCodes = null, string[] fields = null, string[] modelTypes = null)
        {
            return GetEntitiesTextSearch<Entity>(Db.Entities, searchString, languageCodes, fields, modelTypes);
        }

        private string SolrEscape(string input)
        {
            return input.Replace("\"", "\\\"");
        }

        private string GenerateSolrQuery(string searchString, string[] languageCodes, string[] fields, string[] modelTypes)
        {
            StringBuilder query = new StringBuilder("(");

            // Add the value string
            query.AppendFormat("value_txt_{0}:{1}", languageCodes[0], searchString);

            for (int i = 1; i < languageCodes.Length; ++i)
            {
                query.AppendFormat(" OR value_txt_{0}:{1}", languageCodes[1], searchString);
            }

            query.Append(")");

            // Add model types to the query
            if (modelTypes != null && modelTypes.Length > 0)
            {
                query.Append(" AND (");

                query.AppendFormat("modeltype_s:\"{0}\"", SolrEscape(modelTypes[0]));

                for (int i = 1; i < modelTypes.Length; ++i)
                {
                    query.AppendFormat("OR modeltype_s:\"{0}\"", SolrEscape(modelTypes[i]));
                }

                query.Append(")");
            }

            // Limit it to the specific fields
            if (fields != null && fields.Length > 0)
            {
                query.Append(" AND (");

                for (int j = 0; j < languageCodes.Length; ++j)
                {
                    if (j > 0)
                    {
                        query.Append(" OR ");
                    }

                    query.AppendFormat("name_txt_{0}:\"{1}\"", languageCodes[j], SolrEscape(modelTypes[0]));

                    for (int i = 1; i < modelTypes.Length; ++i)
                    {
                        query.AppendFormat("name_txt_{0}:\"{1}\"", languageCodes[j], SolrEscape(modelTypes[i]));
                    }
                }

                query.Append(")");
            }

            return query.ToString();
        }

        protected IQueryable<T> GetEntitiesTextSearch<T>(DbSet<T> Entities, string searchString, string[] languageCodes = null, string[] fields = null, string[] modelTypes = null) where T : Entity
        {
            if(languageCodes == null || languageCodes.Length == 0)
            {
                languageCodes = new string[] { "en" };
            }

            string query = GenerateSolrQuery(searchString, languageCodes, fields, modelTypes);

            return Entities.FromSolr(query);
        }
    }
}

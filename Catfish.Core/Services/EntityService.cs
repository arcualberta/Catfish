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
using Catfish.Core.Contexts;

namespace Catfish.Core.Services
{
    public class EntityService : ServiceBase
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

        public void DisassociateFromAggregations(CFAggregation aggregation)
        {
            // Remove all parent aggregations
            {
                CFAggregation[] parents = aggregation.ParentMembers.ToArray();
                foreach (CFAggregation parent in parents)
                {
                    parent.RemoveChild(aggregation);
                    Db.Entry(parent).State = EntityState.Modified;
                }
            }

            // Remove all child aggregations
            {
                CFAggregation[] children = aggregation.ChildMembers.ToArray();
                foreach (CFAggregation child in children)
                {
                    aggregation.RemoveChild(child);
                    Db.Entry(child).State = EntityState.Modified;
                }
            }

            // Remove all child relationships
            {
                CFItem[] children = aggregation.RelatedMembers.ToArray();
                foreach (CFItem child in children)
                {
                    aggregation.RemoveRelated(child);
                    Db.Entry(child).State = EntityState.Modified;
                }
            }
        }

        public CFEntity GetAnEntity(int id)
        {
            return Db.Entities.Where(e => e.Id == id).FirstOrDefault();
        }

        public IEnumerable<CFEntity> GetEntityParents(int id)
        {
            return Db.Entities.OfType<CFAggregation>()
                .Where(e => e.ConnectionToChildren.Select(c => c.ChildId).Contains(id));
        }

        public T CreateEntity<T>(int entityTypeId) where T : CFEntity, new()
        {
            CFEntityType et = Db.EntityTypes.Where(t => t.Id == entityTypeId).FirstOrDefault();
            if (et == null)
                throw new Exception("EntityType with ID " + entityTypeId + " not found");

            T entity = new T();
            entity.EntityType = et;
            entity.EntityTypeId = et.Id;
            entity.InitMetadataSet(et.MetadataSets.ToList());
            entity.SetAttribute("entity-type", et.Name);

            //removing audit trail entry that was created when creating the metadata set originally
            foreach (CFMetadataSet ms in entity.MetadataSets)
            {
                XElement audit = ms.Data.Element("audit");
                if (audit != null)
                    audit.Remove();
            }

            return entity;
        }

        public CFEntityType CreateEntityType(CFEntityType entityType)
        {
            CFEntityType result = Db.EntityTypes.Add(entityType);
            foreach (var m in entityType.MetadataSets)
            {
                if (m.Id < 1)
                    continue;

                Db.MetadataSets.Attach(m);
            }

            return result;
        }

        public CFEntity UpdateEntity(CFEntity entity)
        {
            Db.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public IQueryable<CFEntity> GetEntitiesTextSearch(string searchString, string[] languageCodes = null, string[] fields = null, string[] modelTypes = null)
        {
            return GetEntitiesTextSearch<CFEntity>(Db.Entities, searchString, languageCodes, fields, modelTypes);
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

        protected IQueryable<T> GetEntitiesTextSearch<T>(DbSet<T> Entities, string searchString, string[] languageCodes = null, string[] fields = null, string[] modelTypes = null) where T : CFEntity
        {
            if (languageCodes == null || languageCodes.Length == 0)
            {
                languageCodes = new string[] { "en" };
            }

            string query = GenerateSolrQuery(searchString, languageCodes, fields, modelTypes);
            int total;

            

            return Entities.FromSolr(query, out total);
        }

        public IEnumerable<CFEntity> GetEntitiesWithMetadataSet(int metadataId)
        {
            var entityTypes = Db.MetadataSets.Where(m => m.Id == metadataId)
                .SelectMany(ms => ms.EntityTypes)
                .Select(e => e.Id);


            var result = Db.Entities.Where(e => entityTypes.Any(i => i == e.EntityTypeId));

            return result;
        }

        private CFEntity UpdateEntityMetadataSet(CFEntity entity, CFMetadataSet metadata)
        {
            CFMetadataSet entityMetadata = entity.MetadataSets.Where(m => m.Guid == metadata.Guid).FirstOrDefault();

            if (entityMetadata != null)
            {
                List<FormField> entityFields = new List<FormField>(metadata.Fields.Count);

                foreach (FormField field in metadata.Fields)
                {
                    FormField entityField = entityMetadata.Fields.Where(f => f.Guid == field.Guid).FirstOrDefault();
                    
                    if(entityField == null)
                    {
                        entityFields.Add(field);
                    }
                    else
                    {
                        entityField.Merge(field);
                        entityFields.Add(entityField);
                    }
                }

                entityMetadata.Fields = entityFields;

                return entity;
            }

            return null;
        }

        public int UpdateExistingEntityMetadata(CFMetadataSet metadata)
        {
            int totalChanged = 0;
            CFEntity result;
            List<CFEntity> entities = GetEntitiesWithMetadataSet(metadata.Id).ToList();

            foreach(CFEntity entity in entities)
            {
                result = UpdateEntityMetadataSet(entity, metadata);

                if (result != null)
                {
                    ++totalChanged;
                    Db.Entry(result).State = EntityState.Modified;
                }
            }

            return totalChanged;
        }

        protected string SortField(int sortAttributeMappingId)
        {

            string sortField = null;
            CFEntityTypeAttributeMapping attrMap = Db.EntityTypeAttributeMappings.Where(m => m.Id == sortAttributeMappingId).FirstOrDefault();

            if (attrMap != null)
            {
                string resultType = "en_ss";
                FormField field = attrMap.Field;

                if (typeof(NumberField).IsAssignableFrom(field.GetType()))
                {
                    resultType = "is";
                    sortField = string.Format("field(value_{0}_{1}_{2}, min)", attrMap.MetadataSet.Guid.Replace('-', '_'), field.Guid.Replace('-', '_'), resultType);
                }
                else
                {
                    sortField = string.Format("value_{0}_{1}_{2}", attrMap.MetadataSet.Guid.Replace('-', '_'), field.Guid.Replace('-', '_'), resultType);
                }

                //return Db.Items.FromSolr(query, out total, entityTypeFilter, start, itemsPerPage,
                //    sortField, sortAsc);
            }
            return sortField;
        }

    }
}

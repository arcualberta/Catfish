using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Metadata;

namespace Catfish.Core.Services
{
    public class MetadataService : ServiceBase
    {
        public MetadataService(CatfishDbContext db) : base(db) { }

        public IEnumerable<MetadataSet> GetMetadataSets()
        {
            List<MetadataSet> ms = Db.MetadataSets.ToList();
            foreach (var m in ms)
                m.Deserialize();
            return ms;
        }

        public MetadataSet GetMetadataSet(int id)
        {
            MetadataSet metadata = Db.MetadataSets.Where(m => m.Id == id).FirstOrDefault();
            if (metadata != null)
                metadata.Deserialize();
            return metadata;
        }

        /// <summary>
        /// Returns the list of metadata filed types that can be used as metadata fileds. 
        /// This excludes base classes that are not directly usable as fields in a form.
        /// </summary>
        /// <returns></returns>
        public List<Type> GetMetadataFieldTypes()
        {
            var fieldTypes = typeof(FormField).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(FormField)) 
                    && !t.CustomAttributes.Where(a => a.AttributeType.IsAssignableFrom(typeof(IgnoreAttribute))).Any())
                .ToList();


            return fieldTypes;
        }

        public MetadataSet UpdateMetadataSet(MetadataSet metadataSet)
        {
            ////MetadataSet ms;
            if (metadataSet.Id > 0)
            {
                ////ms = Db.MetadataSets.Where(m => m.Id == metadataDefinition.Id).FirstOrDefault();
                ////if (ms == null)
                ////    return null;

                Db.Entry(metadataSet).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                ////ms = new MetadataSet();
                Db.MetadataSets.Add(metadataSet);
            }
            ////ms.Definition = metadataDefinition;
            return metadataSet;
        }
    }
}

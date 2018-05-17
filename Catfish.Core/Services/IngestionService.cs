using Catfish.Core.Models;
using Catfish.Core.Models.Ingestion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Catfish.Core.Services
{
    public class IngestionService : ServiceBase
    {
        public IngestionService(CatfishDbContext db) : base(db)
        {

        }

        public IngestionService Parse(XElement ingestion)
        {
            throw new NotImplementedException();
        }

        private string NewGuid()
        {
            return System.Guid.NewGuid().ToString("N");
        }

        
        public void Import(Ingestion ingestion)
        {
#if DEBUG
            Console.Error.WriteLine("Starting ingestion of Ingestion object.");
#endif

            //create new GUID and new EntityType-Id
            Dictionary<string, string> GuidMap = new Dictionary<string, string>();
            Dictionary<int, int> IdMap = new Dictionary<int, int>();
           
            
            foreach (CFMetadataSet ms in ingestion.MetadataSets)
            {
                string oldId = ms.Guid;
                string newGuid = NewGuid();
                var meta = ms;
                if (ingestion.Overwrite)
                {
                    //use whatever GUID in the file
                    //check to make sure the GUID in not existing in the Db, if it's replace the one in the database
                    GuidMap.Add(oldId, oldId); //if overwrite is true use existing GUID
                }
                else
                {
                    GuidMap.Add(oldId, newGuid);
                }
                //GuidMap = buildMapGuid(ingestion.Overwrite, oldId);
                ms.Guid = GuidMap[oldId];
                ms.MappedGuid = GuidMap[oldId];

                 ms.Serialize();
                //meta.Serialize();
                if (ingestion.Overwrite)
                {
                    //check if the metadataset with this guid is existed in the database
                    //if yes, replace the one in th edatabase with this one from the file
                    CFMetadataSet metadataSet = Db.MetadataSets.Where(m => m.MappedGuid == ms.Guid).FirstOrDefault();
                    if(metadataSet != null)
                    {
                        metadataSet = ms;
                        Db.Entry(metadataSet).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Db.MetadataSets.Add(ms);
                    }

                }
                else
                {
                    Db.MetadataSets.Add(ms);
                }
            }

            Db.SaveChanges();
            //add entityType
            foreach (CFEntityType et in ingestion.EntityTypes)
            {
                int oldId = et.Id;

                //I think below is not necessary since the DeserialiseEntytyType mtehod already grabbing the right metadataset

                List<CFMetadataSet> newSets = new List<CFMetadataSet>();
                foreach (CFMetadataSet set in et.MetadataSets)
                {
                    string mGuid = set.Guid;
                    if (GuidMap.ContainsKey(mGuid))
                    {
                        mGuid = GuidMap[mGuid];
                    }

                    CFMetadataSet dbSet = Db.MetadataSets.Where(s => s.MappedGuid == mGuid).FirstOrDefault();
                    newSets.Add(dbSet);
                }
                et.MetadataSets.Clear();
                et.MetadataSets = newSets;

                foreach (CFEntityTypeAttributeMapping mapping in et.AttributeMappings)
                {
                    string mGuid = mapping.MetadataSet.Guid;
                    if (GuidMap.ContainsKey(mGuid))
                    {
                        mGuid = GuidMap[mGuid];
                    }
                    mapping.MetadataSet = Db.MetadataSets.Where(m => m.MappedGuid == mGuid).FirstOrDefault();
                }

                CFEntityType newEt=null;
                if (ingestion.Overwrite)
                {
                    CFEntityType oldEt = Db.EntityTypes.Where(e => e.Name == et.Name).FirstOrDefault();
                    if(oldEt != null)
                    {
                        //modified it --replace with current value
                        oldEt = et;
                        Db.Entry(oldEt).State = System.Data.Entity.EntityState.Modified;       
                    }
                    else
                    {
                        newEt = Db.EntityTypes.Add(et);
                    }
                }
                else
                {
                    newEt = Db.EntityTypes.Add(et);
                }                     
                Db.SaveChanges();
                IdMap.Add(oldId, newEt.Id);
            }

            //add aggregations
            GuidMap.Clear();
            foreach (var agg in ingestion.Aggregations)
            {
                string oldId = agg.Guid;
                string newGuid = NewGuid();

                if (ingestion.Overwrite)
                {
                    //use whatever GUID in the file
                    //check to make sure the GUID in not existing in the Db, if it's replace the one in the database
                    GuidMap.Add(oldId, oldId); //if overwrite is true use existing GUID
                }
                else
                {
                    agg.Guid = newGuid;
                    agg.MappedGuid = newGuid;

                    GuidMap.Add(oldId, newGuid);
                }

                //saving the aggregation object
                if (ingestion.Overwrite)
                {
                    CFAggregation _aggregation = Db.XmlModels.Where(a => a.MappedGuid == newGuid).FirstOrDefault() as CFAggregation;
                    if(_aggregation != null)
                    {
                        _aggregation =(CFAggregation) agg;
                        Db.Entry(_aggregation).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        Type t = agg.GetType();
                        MethodInfo method = this.GetType().GetMethod("CreateAggregation");
                        MethodInfo genMethod = method.MakeGenericMethod(t);
                        var _agg = (CFAggregation)genMethod.Invoke(this, new object[] { agg });
                        Db.Entities.Add(_agg);
                    }
                }
                else
                {
                    Type t = agg.GetType();
                    MethodInfo method = this.GetType().GetMethod("CreateAggregation");
                    MethodInfo genMethod = method.MakeGenericMethod(t);
                    var _agg = (CFAggregation)genMethod.Invoke(this, new object[] { agg });
                    Db.Entities.Add(_agg);
                }
                
            }
            Db.SaveChanges();
            foreach (var rel in ingestion.Relationships)
            { 
                string pGuid = rel.ParentRef;                
                if (GuidMap.ContainsKey(pGuid))
                {
                    pGuid = GuidMap[pGuid];
                }

                string cGuid = rel.ChildRef;   
                if (GuidMap.ContainsKey(cGuid))
                {
                    cGuid = GuidMap[cGuid];
                }

                rel.ParentRef = pGuid;
                rel.ChildRef = cGuid;
                if (rel.IsMember)
                {    
                    AddMember(rel.ParentRef, rel.ChildRef);
                }
                
                if(rel.IsRelated)
                {
                    //save it in Aggregation has related
                    AddRelatedMember(rel.ParentRef, rel.ChildRef);
                }
            }
        }

        
        private void AddMember(string parentGuid, string childGuid, bool overwrite = false)
        {
            try
            {
                CFAggregation parent = Db.XmlModels.Where(x => x.MappedGuid == parentGuid).FirstOrDefault() as CFAggregation;
                CFAggregation child = Db.XmlModels.Where(x => x.MappedGuid == childGuid).FirstOrDefault() as CFAggregation;
                if(!overwrite)
                { parent.ChildMembers.Add(child); }
                else
                {
                    //remove all child members first before adding new one
                    foreach(var c in parent.ChildMembers)
                    {
                        CFAggregation removeChild = Db.XmlModels.Where(x => x.Id == c.Id).FirstOrDefault() as CFAggregation;
                        parent.ChildMembers.Remove(removeChild);
                    }

                    //add new one
                    parent.ChildMembers.Add(child);
                }
            
                
                Db.Entry(parent).State = System.Data.Entity.EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private void AddRelatedMember(string parentGuid, string childGuid, bool overwrite=false)
        {
            try
            {
                CFAggregation parent = Db.XmlModels.Where(x => x.MappedGuid == parentGuid).FirstOrDefault() as CFAggregation;
                CFItem child = Db.XmlModels.Where(x => x.MappedGuid == childGuid).FirstOrDefault() as CFItem;
                if(!overwrite)
                   parent.ChildRelations.Add(child);
                else
                {
                    //remove all child members first before adding new one
                    foreach (var c in parent.ChildRelations)
                    {
                        CFItem removeRel = Db.XmlModels.Where(x => x.Id == c.Id).FirstOrDefault() as CFItem;
                        parent.ChildRelations.Remove(removeRel);
                    }

                    //add new one
                    parent.ChildRelations.Add(child);
                }

                Db.Entry(parent).State = System.Data.Entity.EntityState.Modified;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public T CreateAggregation<T>(CFXmlModel aggregation) where T : CFAggregation, new()
        {  
            T agg = new T();
            string entityTypeName = aggregation.Data.Attribute("entity-type").Value;
            CFEntityType entType = Db.EntityTypes.Where(e => e.Name == entityTypeName).FirstOrDefault();
            agg = (T)aggregation;
            agg.EntityType = entType;
           
            return agg;
        }

       

        public void Import(XElement ingestion)
        {
            Ingestion input = new Ingestion();
            input.Deserialize(ingestion);

            Import(input);
        }

        public void Import(Stream ingestion)
        {
#if DEBUG
            Console.Error.WriteLine("Converting ingestion stream to Ingestion object.");
#endif
            Ingestion ing = new Ingestion();

            ing.Deserialize(ingestion);
            
            Import(ing);
        }

        public Ingestion Export()
        {
            IEnumerable<CFCollection> collections = Db.Collections;
            IEnumerable<CFItem> items = Db.Items;
            IEnumerable<CFEntityType> entitytypes = Db.EntityTypes;
            IEnumerable<CFMetadataSet> metadatasets = Db.MetadataSets;
            //IEnumerable<Form> forms = Db.FormTemplates;

            Ingestion ingestion = new Ingestion();
            ingestion.MetadataSets.AddRange(metadatasets);
            ingestion.EntityTypes.AddRange(entitytypes);
            ingestion.Aggregations.AddRange(collections);
            ingestion.Aggregations.AddRange(items);
            //ingestion.Aggregations.AddRange(forms);   //MR Feb 23 2018: Form is not an Aggregation object: ignore form now -- have to revisit this later


            CFItem[] itemArray = items.ToArray();

            //find all item member in each collection
            foreach(CFCollection col in collections.ToList())
            {
                foreach(CFItem itm in items.ToList())
                {
                    if(col.ChildMembers.Any(p=>p.MappedGuid == itm.Guid))
                    {
                        ingestion.Relationships.Add(new Relationship
                        {
                            ParentRef = col.Guid,
                            ChildRef = itm.Guid,
                            IsMember = true,
                            IsRelated = false
                        });
                    }
                }
            }

            //find all item member in each item
            foreach (CFItem parentItem in items.ToList())
            {
                foreach (CFItem childItem in items.ToList())
                {
                    if (parentItem.ChildMembers.Any(member=>member.MappedGuid == childItem.Guid))
                    {
                        ingestion.Relationships.Add(new Relationship
                        {
                            ParentRef = parentItem.Guid,
                            ChildRef = childItem.Guid,
                            IsMember = true,
                            IsRelated = false
                        });
                    }

                    //find related items
                    if (parentItem.ChildRelations.Any(member => member.MappedGuid == childItem.Guid))
                    {
                        ingestion.Relationships.Add(new Relationship
                        {
                            ParentRef = parentItem.Guid,
                            ChildRef = childItem.Guid,
                            IsMember = true,
                            IsRelated = true
                        });
                    }
                }
            }

            return ingestion;

        }

        public void Export(Stream stream, Ingestion ingestion = null)
        {
            if (ingestion == null)
            {
                ingestion = Export();
            }

            XElement element = ingestion.Serialize();
            element.Save(stream);
        }
    }
}

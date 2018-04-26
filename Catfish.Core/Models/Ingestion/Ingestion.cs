using Catfish.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Ingestion
{
    public class Ingestion
    {
        public bool Overwrite { get; set; }

        public List<MetadataSet> MetadataSets { get; set; }

        public List<CFEntityType> EntityTypes { get; set; }

        public List<XmlModel> Aggregations { get; set; }

        public List<Relationship> Relationships { get; set; }

        public Ingestion()
        {
            Overwrite = false;
            MetadataSets = new List<MetadataSet>();
            EntityTypes = new List<CFEntityType>();
            Aggregations = new List<XmlModel>();
            Relationships = new List<Relationship>();
        }

        public XElement Serialize()
        {
            XElement result = new XElement("ingestion");
            result.SetAttributeValue("overwrite", Overwrite);

            result.Add(SerializeMetadatasets());
            result.Add(SerializeEntityTypes());
            result.Add(SerializeAggrigaitons());
            result.Add(SerializeRelationships());

            return result;
        }

        public Ingestion Deserialize(XElement ingestion)
        {
            if (ingestion.Name != "ingestion")
            {
                throw new FormatException("Invalid XML relationship element.");
            }

            foreach(XAttribute attribute in ingestion.Attributes())
            {
                switch (attribute.Name.LocalName)
                {
                    case "overwrite":
                        Overwrite = bool.Parse(attribute.Value);
                        break;
                }
            }

            foreach(XElement element in ingestion.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "metadata-sets":
                        DeserializeMetadatasets(element);
                        break;

                    case "entity-types":
                        DeserializeEntityTypes(element);
                        break;

                    case "aggregations":
                        DeserializeAggregations(element);
                        break;

                    case "relationships":
                        DeserializeRelationships(element);
                        break;
                }
            }

            return this;
        }

        private XElement SerializeMetadatasets()
        {
            XElement result = new XElement("metadata-sets");

            foreach(MetadataSet set in MetadataSets)
            {
                XElement metadataset = XElement.Parse(set.Content);
                result.Add(metadataset);
            }

            return result;
        }

        private Ingestion DeserializeMetadatasets(XElement element)
        {
            foreach(XElement setElement in element.Elements())
            {
                string name = setElement.Name.LocalName;
                string strGuid = setElement.Attribute("guid").Value;
                if (name == "metadata-set")
                {
                    MetadataSet set = new MetadataSet();
                    set.Content = setElement.ToString();
                    set.Guid = strGuid;
                    set.MappedGuid = strGuid;
                    MetadataSets.Add(set);
                }
            }

            return this;
        }

        private XElement SerializeEntityTypes()
        {
            XElement result = new XElement("entity-types");

            foreach(CFEntityType type in EntityTypes)
            {
                XElement entityType = new XElement("entity-type");
                entityType.SetAttributeValue("id", type.Id);

                XElement element = new XElement("name");
                element.SetValue(type.Name == null ? String.Empty : type.Name);
                entityType.Add(element);

                element = new XElement("description");
                element.SetValue(type.Description == null ? String.Empty : type.Description);
                entityType.Add(element);

                element = new XElement("target-type");
                element.SetValue(type.TargetTypes == null ? String.Empty : type.TargetTypes);
                entityType.Add(element);

                element = new XElement("metadata-sets");
                foreach(MetadataSet set in type.MetadataSets)
                {
                    XElement metadataSet = new XElement("metadata-set");
                    metadataSet.SetAttributeValue("ref", set.Guid);

                    IEnumerable<EntityTypeAttributeMapping> mappings = type.AttributeMappings.Where(m => m.MetadataSet.Guid == set.Guid);
                    foreach(EntityTypeAttributeMapping map in mappings)
                    {
                        XElement attributeMapping = new XElement("attribute-mapping");

                        XElement childElement = new XElement("Name");
                        childElement.SetValue(map.Name);
                        attributeMapping.Add(childElement);

                        childElement = new XElement("field-name");
                        childElement.SetValue(map.FieldName);
                        attributeMapping.Add(childElement);

                        metadataSet.Add(attributeMapping);
                    }

                    element.Add(metadataSet);
                }
                entityType.Add(element);

                result.Add(entityType);
            }

            return result;
        }

        private Ingestion DeserializeEntityTypes(XElement element)
        {
            foreach(XElement entityElement in element.Elements())
            {
                CFEntityType entityType = new CFEntityType();
                entityType.Id = entityElement.Attribute("id") == null ? 0 : int.Parse(entityElement.Attribute("id").Value);
                
                foreach(XElement child in entityElement.Elements())
                {
                    string name = child.Name.LocalName;

                    if(name == "name")
                    {
                        entityType.Name = child.Value;
                    }else if(name == "description")
                    {
                        entityType.Description = child.Value;
                    }else if(name == "target-type")
                    {
                        entityType.TargetTypes = child.Value;
                    }else if(name == "metadata-sets")
                    {
                        foreach(XElement metadata in child.Elements())
                        {
                            if (metadata.Name.LocalName == "metadata-set")
                            {
                                //MetadataSet set = MetadataSets.Where(m => m.Guid == metadata.Attribute("ref").Value).FirstOrDefault();
                                //if (set == null)
                                //{
                                    MetadataSet set = new MetadataSet();
                                    set.Guid = metadata.Attribute("ref").Value;
                                    set.Id = -1;
                                //}

                                entityType.MetadataSets.Add(set);

                                foreach (XElement attrElement in metadata.Elements())
                                {
                                    if(attrElement.Name.LocalName == "attribute-mapping")
                                    {
                                        EntityTypeAttributeMapping mapping = new EntityTypeAttributeMapping();
                                        mapping.MetadataSet = set;

                                        foreach(XElement attrChild in attrElement.Elements())
                                        {
                                            string attrName = attrChild.Name.LocalName;
                                            if(attrName == "name")
                                            {
                                                mapping.Name = attrChild.Value;
                                            }
                                            else if(attrName == "field-name")
                                            {
                                                mapping.FieldName = attrChild.Value;
                                            }
                                            else if(attrName == "label")
                                            {
                                                //When we implement the new mappings in a later sprint, we will need this field.
                                            }
                                        }

                                        entityType.AttributeMappings.Add(mapping);
                                    }
                                }
                            }
                        }
                    }
                }

                EntityTypes.Add(entityType);
            }

            return this;
        }

        private XElement SerializeAggrigaitons()
        {
            XElement result = new XElement("aggregations");

            foreach(CFAggregation aggregation in Aggregations)
            {
                if (aggregation.Content != null)
                {
                    XElement child = XElement.Parse(aggregation.Content);
                    result.Add(child);
                }
            }

            return result;
        }

        private Ingestion DeserializeAggregations(XElement element)
        {
            foreach(XElement child in element.Elements())
            {
                string name = child.Name.LocalName;
                XmlModel model = null;
                string strGuid = child.Attribute("guid").Value;
                switch (name)
                {
                    case "collection":
                        model = new CFCollection();
                        break;

                    case "item":
                        model = new Item();
                        break;

                    case "form":
                        model = new Form();
                        break;

                    case "file":
                        model = new DataFile();
                        break;
                }

                if(model != null)
                {
                    model.Guid = strGuid;
                    model.MappedGuid = strGuid;
                    model.Content = child.ToString();
                    Aggregations.Add(model);
                }
            }
            return this;
        }

        private XElement SerializeRelationships()
        {
            XElement result = new XElement("relationships");

            foreach(Relationship relationship in Relationships)
            {
                result.Add(relationship.Serialize());
            }

            return result;
        }

        private Ingestion DeserializeRelationships(XElement element)
        {
            foreach (XElement child in element.Elements())
            {
                if(child.Name.LocalName == "relationship")
                {
                    Relationships.Add(new Relationship().Deserialize(child));
                }
            }
            return this;
        }
    }
}

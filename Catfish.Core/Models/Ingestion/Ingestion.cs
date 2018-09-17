using Catfish.Core.Helpers;
using Catfish.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Catfish.Core.Models.Ingestion
{
    public class Ingestion
    {
        public bool Overwrite { get; set; }

        public List<CFMetadataSet> MetadataSets { get; set; }

        public List<CFEntityType> EntityTypes { get; set; }

        public BigList<CFXmlModel> Aggregations { get; set; }

        public IList<Relationship> Relationships { get; set; }

        public Ingestion(int pageSize = 10000)
        {
            Overwrite = false;
            MetadataSets = new List<CFMetadataSet>();
            EntityTypes = new List<CFEntityType>();
            Aggregations = new BigList<CFXmlModel>(pageSize);
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

        public Ingestion Deserialize(Stream input)
        {
            bool isIngestion = false;
            using(System.Xml.XmlReader reader = System.Xml.XmlReader.Create(input))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (!isIngestion)
                        {
                            if(reader.Name != "ingestion")
                            {
                                throw new FormatException("Invalid XML relationship element.");
                            }

                            isIngestion = true;

                            string overwrite = reader.GetAttribute("overwrite");
                            if (!string.IsNullOrWhiteSpace(overwrite))
                            {
                                Overwrite = bool.Parse(overwrite);
                            }
                            else
                            {
                                Overwrite = false;
                            }
                        }

                        switch (reader.Name)
                        {
                            case "ingestion":
                                break;

                            case "metadata-sets":
                                DeserializeMetadatasets(reader);
                                break;

                            case "aggregations":
                                DeserializeAggregations(reader);
                                break;

                            case "relationships":
                                DeserializeRelationships(reader);
                                break;

                            case "entity-types":
                                DeserializeEntityTypes(reader);
                                break;

                            default:
                                throw new FormatException("Invalid XML element: " + reader.Name);
                        }
                    }
                }
            }

            return this;
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

            foreach(CFMetadataSet set in MetadataSets)
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
                    CFMetadataSet set = new CFMetadataSet();
                    set.Content = setElement.ToString();
                    set.Guid = strGuid;
                    set.MappedGuid = strGuid;
                    MetadataSets.Add(set);
                }
            }

            return this;
        }

        private Ingestion DeserializeMetadatasets(System.Xml.XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if(reader.Name == "metadata-set")
                    {
                        string strGuid = reader.GetAttribute("guid");
                        CFMetadataSet set = new CFMetadataSet();
                        set.Content = reader.ReadOuterXml();
                        set.Guid = strGuid;
                        set.MappedGuid = strGuid;
                        MetadataSets.Add(set);
                    }
                    else
                    {
                        throw new FormatException("Invalid XML element: " + reader.Name);
                    }
                }

                if(reader.NodeType == System.Xml.XmlNodeType.EndElement)
                {
                    if(reader.Name == "metadata-sets")
                    {
                        return this;
                    }
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
                foreach(CFMetadataSet set in type.MetadataSets)
                {
                    XElement metadataSet = new XElement("metadata-set");
                    metadataSet.SetAttributeValue("ref", set.Guid);

                    IEnumerable<CFEntityTypeAttributeMapping> mappings = type.AttributeMappings.Where(m => m.MetadataSet.Guid == set.Guid);
                    foreach(CFEntityTypeAttributeMapping map in mappings)
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

        private Ingestion DeserializeEntityTypes(System.Xml.XmlReader reader)
        {
            return DeserializeEntityTypes(XElement.Parse(reader.ReadOuterXml()));
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
                                    CFMetadataSet set = new CFMetadataSet();
                                    set.Guid = metadata.Attribute("ref").Value;
                                    set.Id = -1;
                                //}

                                entityType.MetadataSets.Add(set);

                                foreach (XElement attrElement in metadata.Elements())
                                {
                                    if(attrElement.Name.LocalName == "attribute-mapping")
                                    {
                                        CFEntityTypeAttributeMapping mapping = new CFEntityTypeAttributeMapping();
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

        private Ingestion DeserializeAggregations(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    string name = reader.LocalName;
                    CFXmlModel model = null;
                    string strGuid = reader.GetAttribute("guid");
                    switch (name)
                    {
                        case "collection":
                            model = new CFCollection();
                            break;

                        case "item":
                            model = new CFItem();
                            break;

                        case "form":
                            model = new Form();
                            break;

                        case "file":
                            model = new CFDataFile();
                            break;

                        default:
                            throw new FormatException("Invalid XML element: " + reader.Name);
                    }

                    if (model != null)
                    {
                        model.Guid = strGuid;
                        model.MappedGuid = strGuid;
                        model.Content = reader.ReadOuterXml();
                        Aggregations.Add(model);
                    }
                }

                if (reader.NodeType == System.Xml.XmlNodeType.EndElement)
                {
                    if (reader.Name == "aggregations")
                    {
                        return this;
                    }
                }
            }

            return this;
        }

        private Ingestion DeserializeAggregations(XElement element)
        {
            foreach(XElement child in element.Elements())
            {
                string name = child.Name.LocalName;
                CFXmlModel model = null;
                string strGuid = child.Attribute("guid").Value;
                switch (name)
                {
                    case "collection":
                        model = new CFCollection();
                        break;

                    case "item":
                        model = new CFItem();
                        break;

                    case "form":
                        model = new Form();
                        break;

                    case "file":
                        model = new CFDataFile();
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

        private Ingestion DeserializeRelationships(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name == "relationship")
                    {
                        XElement element = XElement.Parse(reader.ReadOuterXml());
                        Relationships.Add(new Relationship().Deserialize(element));
                    }
                    else
                    {
                        throw new FormatException("Invalid XML element: " + reader.Name);
                    }
                }

                if (reader.NodeType == System.Xml.XmlNodeType.EndElement)
                {
                    if (reader.Name == "relationships")
                    {
                        return this;
                    }
                }
            }

            return this;
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

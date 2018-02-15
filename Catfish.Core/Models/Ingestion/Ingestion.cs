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

        public List<EntityType> EntityTypes { get; set; }

        public List<Aggregation> Aggregations { get; set; }

        public List<Relationship> Relationships { get; set; }

        public Ingestion()
        {
            Overwrite = false;
            MetadataSets = new List<MetadataSet>();
            EntityTypes = new List<EntityType>();
            Aggregations = new List<Aggregation>();
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
            return this;
        }

        private XElement SerializeEntityTypes()
        {
            XElement result = new XElement("entity-types");

            foreach(EntityType type in EntityTypes)
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
            return this;
        }

        private XElement SerializeAggrigaitons()
        {
            XElement result = new XElement("aggregations");

            foreach(Aggregation aggregation in Aggregations)
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
            return this;
        }
    }
}

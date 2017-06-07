using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Catfish.Core.Models.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Xml.Serialization;

namespace Catfish.Core.Models.Metadata
{
    [TypeLabel("Metadata Set")]
    public class MetadataSet
    {
        public int Id { get; set; }

        [Column(TypeName = "xml")]
        public string Content { get; set; }

        private MetadataDefinition mDefinition;
        [NotMapped]
        public MetadataDefinition Definition
        {
            get
            {
                if(mDefinition == null)
                {
                    if (string.IsNullOrEmpty(Content))
                    {
                        mDefinition = new MetadataDefinition();
                        Serialize();
                    }
                    else
                    {
                        Deserialize();
                    }
                }
                return mDefinition;
            }

            set
            {
                mDefinition = value;
            }
        }

        public void Serialize()
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MetadataDefinition));
                serializer.Serialize(writer, mDefinition);
                Content = writer.ToString();
            }
        }

        public void Deserialize()
        {
            using (StringReader reader = new StringReader(Content))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MetadataDefinition));
                mDefinition = serializer.Deserialize(reader) as MetadataDefinition;
            }
        }

        [NotMapped]
        [TypeLabel("String")]
        public string Name { get { return Definition.Name; } }

        [DataType(DataType.MultilineText)]
        public string Description { get { return Definition.Description; } }

        public virtual ICollection<SimpleField> Fields { get; set; }

        public virtual ICollection<EntityType> EntityTypes { get; set; }

        public MetadataSet()
        {
            Fields = new List<SimpleField>();
        }
    }
}

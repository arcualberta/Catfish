﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Catfish.Core.Models.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Catfish.Core.Models.Metadata
{
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
                mDefinition.Id = this.Id;
            }
        }

        public void Serialize()
        {
            XElement xml = Definition.ToXml();
            Content = xml.ToString();
            ////using (StringWriter writer = new StringWriter())
            ////{
            ////    XmlSerializer serializer = new XmlSerializer(typeof(MetadataDefinition));
            ////    serializer.Serialize(writer, mDefinition);
            ////    Content = writer.ToString();
            ////}
        }

        public void Deserialize()
        {
            XElement xml = XElement.Parse(Content);
            mDefinition = XmlModel.Parse(xml) as MetadataDefinition;
            mDefinition.Id = this.Id;

            ////using (StringReader reader = new StringReader(Content))
            ////{
            ////    XmlSerializer serializer = new XmlSerializer(typeof(MetadataDefinition));
            ////    mDefinition = serializer.Deserialize(reader) as MetadataDefinition;
            ////}
            ////mDefinition.Id = this.Id;
        }

        [NotMapped]
        [TypeLabel("String")]
        public string Name { get { return Definition.Name; } }

        [DataType(DataType.MultilineText)]
        public string Description { get { return Definition.Description; } }

        ////public virtual ICollection<SimpleField> Fields { get; set; }

        public virtual ICollection<EntityType> EntityTypes { get; set; }

        public MetadataSet()
        {
            ////Fields = new List<SimpleField>();
        }
    }
}

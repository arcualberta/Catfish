using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Catfish.Core.Models.Attributes;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;

namespace Catfish.Core.Models.Metadata
{
    public class MetadataSet : XmlModel
    {
        public override string GetTagName() { return "metadata-set"; }

        [NotMapped]
        public List<MetadataField> Fields
        {
            get
            {
                return GetChildModels("fields/field", Data).Select(c => c as MetadataField).ToList();
            }

            set
            {
                //Removing all children inside the metadata set element
                RemoveAllElements("fields/field", Data);

                foreach (MetadataField ms in value)
                    InsertChildElement("./fields", ms.Data);
            }
        }

        ////[NotMapped]
        ////public virtual List<MetadataField> Fields
        ////{
        ////    get
        ////    {
        ////        if (mFields == null)
        ////        {
        ////            if (Data != null)
        ////            {
        ////                List<XmlModel> fields = GetChildModels("fields/field", Data);
        ////                mFields = fields.Select(f => f as MetadataField).ToList();
        ////            }
        ////            else
        ////            {
        ////                mFields = new List<MetadataField>();
        ////            }
        ////        }
        ////        return mFields;
        ////    }
        ////    //set;
        ////}

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
            //XElement xml = Definition.ToXml();
            Content = Data.ToString();
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
            Initialize(xml);
            ////////mDefinition = XmlModel.Parse(xml) as MetadataDefinition;
            ////////mDefinition.Id = this.Id;
        }

        [NotMapped]
        [TypeLabel("String")]
        public string Name { get { return GetName(); } }

        [DataType(DataType.MultilineText)]
        public string Description { get { return GetDescription(); } }

        ////public virtual ICollection<SimpleField> Fields { get; set; }

        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual ICollection<EntityType> EntityTypes { get; set; }

        public MetadataSet()
        {
            Data.Add(new XElement("fields"));
        }

        public override void UpdateValues(XmlModel src)
        {
            base.UpdateValues(src);

            var src_item = src as MetadataSet;

            foreach (MetadataField field in this.Fields)
            {
                var src_field = src_item.Fields.Where(x => x.Ref == field.Ref).FirstOrDefault();
                field.UpdateValues(src_field);
            }
        }
    }
}

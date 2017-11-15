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
    [TypeLabel("Metadata Set")]
    public class MetadataSet : XmlModel
    {
        public override string GetTagName() { return "metadata-set"; }

        [NotMapped]
        public List<FormField> Fields
        {
            get
            {
                return GetChildModels("fields/field", Data).Select(c => c as FormField).ToList();
            }

            set
            {
                //Removing all children inside the metadata set element
                RemoveAllElements("fields/field", Data);

                foreach (FormField ms in value)
                    InsertChildElement("./fields", ms.Data);
            }
        }



        private MetadataDefinition mDefinition;

        [ScriptIgnore(ApplyToOverrides = true)]
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

        [NotMapped]
        [TypeLabel("String")]
        public string Name { get { return GetName(); } set { SetName(value); } }

        [NotMapped]
        [DataType(DataType.MultilineText)]
        public string Description { get { return GetDescription(); } set { SetDescription(value); } }

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

            foreach (FormField field in this.Fields)
            { // checkhere type of 
                var src_field = src_item.Fields.Where(x => x.Ref == field.Ref).FirstOrDefault();
                field.UpdateValues(src_field);
            }
        }

        public void SetFieldValue(string fieldName, string fieldValue, string language)
        {
            SetFieldValue(fieldName, new List<string> { fieldValue }, language);
        }
        public void SetFieldValue(string fieldName, IEnumerable<string> fieldValues, string language)
        {
            FormField field = Fields.Where(f => f.Name == fieldName).FirstOrDefault();
            field.SetValues(fieldValues, language);
        }

    }
}

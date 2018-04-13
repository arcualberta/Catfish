using System;
using System.Collections;
using System.Collections.Generic;
using Catfish.Core.Models.Forms;
using System.Web.Script.Serialization;
using Catfish.Core.Models.Attributes;
using System.Xml.Linq;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Catfish.Core.Helpers;
using Catfish.Core.Models.Access;

namespace Catfish.Core.Models
{
    public abstract class Entity : XmlModel
    {
        public int? EntityTypeId { get; set; }
        public virtual EntityType EntityType { get; set; }

        protected static string AccessGroupXPath = "access/" + AccessGroup.TagName;
        protected static string MetadataSetXPath = "metadata/" + MetadataSet.TagName;

        public Entity()
        {
            Data.Add(new XElement("metadata"));
            Data.Add(new XElement("access"));
        }

        [NotMapped]
        public List<MetadataSet> MetadataSets
        {
            get
            {
                return GetChildModels(MetadataSetXPath)
                    .Select(c => c as MetadataSet).ToList();
            }

            set
            {
                RemoveAllMetadataSets();
                InitMetadataSet(value);
            }

        }

        [NotMapped]
        public List<AccessGroup> AccessGroups
        {
            get
            {

                return GetChildModels(AccessGroupXPath)
                    .Select(c => c as AccessGroup).ToList();
            }

            set
            {
                RemoveAllElements(AccessGroupXPath);
                InitModels("access", value);
            }
        }

        public void RemoveAllMetadataSets()
        {
            //Removing all children inside the metadata set element
            RemoveAllElements(MetadataSetXPath);
        }

        public void InitMetadataSet(IReadOnlyList<MetadataSet> src)
        {
            XElement metadata = GetImmediateChild("metadata");
            foreach (MetadataSet ms in src)
                metadata.Add(ms.Data);
        }

        private void InitModels(string element, IReadOnlyList<XmlModel> models)
        {
            XElement access = GetImmediateChild(element);
            foreach (AccessGroup model in models)
            {
                access.Add(model.Data);
            }
        }

        protected FormField GetMetadataSetField(string metadatasetGuid, string fieldName)
        {
            MetadataSet metadataSet = MetadataSets.Where(ms => ms.Guid == metadatasetGuid).FirstOrDefault();

            if (metadataSet == null)
            {
                return null;
            }

            FormField field = metadataSet.Fields.Where(f => f.Name == fieldName).FirstOrDefault();

            return field;
        }

        public string GetAttributeMappingValue(string name, string lang = null)
        {
            var mapping = EntityType.AttributeMappings.Where(m => m.Name == name).FirstOrDefault();
            if (mapping != null)
            {
                string msGuid = mapping.MetadataSet.Guid;
                string fieldName = mapping.FieldName;

                FormField field = GetMetadataSetField(msGuid, fieldName);

                if (field == null)
                {
                    return string.Format("ERROR: INCORRECT {0} MAPPING FOUND FOR THIS ENTITY TYPE", mapping);
                }

                return MultilingualHelper.Join(field.GetValues(), " / ", false);
            }

            return null;
        }

        public string GetAttributeMappingLabel(string name, string lang = null)
        {
            var mapping = EntityType.AttributeMappings.Where(m => m.Name == name).FirstOrDefault();
            if (mapping != null)
            {
                string msGuid = mapping.MetadataSet.Guid;
                string fieldName = string.IsNullOrEmpty(mapping.Label) ? mapping.FieldName : mapping.Label;

                return fieldName;
            }

            return null;
        }

        protected void SetAttributeMappingValue(string name, string val, string lang = null)
        {
            EntityTypeAttributeMapping mapping = EntityType.AttributeMappings.Where(m => m.Name == name).FirstOrDefault();
            if (mapping == null)
                throw new Exception(string.Format("{0} mapping metadata set is not specified for this entity type", name));

            if (string.IsNullOrEmpty(mapping.FieldName))
                throw new Exception(string.Format("Field is not specified in the {0} Mapping of this entity type", name));

            MetadataSet metadataSet = MetadataSets.Where(ms => ms.Guid == mapping.MetadataSet.Guid).FirstOrDefault();
            metadataSet.SetFieldValue(mapping.FieldName, val, lang);
        }

        public string GetName(string lang = null)
        {
            string result = GetAttributeMappingValue("Name Mapping", lang);

            if (result != null)
            {
                return result;
            }

            return GetChildText("name", Data, Lang(lang));
        }
        public override void SetName(string val, string lang = null)
        {
            SetAttributeMappingValue("Name Mapping", val, lang);
        }

        public override string GetDescription(string lang = null)
        {
            string result = GetAttributeMappingValue("Description Mapping", lang);

            if (result != null)
            {
                return result;
            }

            return GetChildText("description", Data, Lang(lang));
        }
        public override void SetDescription(string val, string lang = null)
        {
            SetAttributeMappingValue("Description Mapping", val, lang);
        }

        public override void UpdateValues(XmlModel src)
        {
            if (src == this)
            {
                // Updating will delete the child content. Since it's the same, we will return without any changes.
                return;
            }

            base.UpdateValues(src);

            var src_item = src as Entity;

            foreach (MetadataSet ms in this.MetadataSets)
            {
                var src_ms = src_item.MetadataSets.Where(x => x.Guid == ms.Guid).FirstOrDefault();
                ms.UpdateValues(src_ms);
            }
        }

        public override string Name
        {
            get
            {
                return GetName();
            }
        }

        public override string Description
        {
            get
            {
                return GetDescription();
            }
        }

    }
}
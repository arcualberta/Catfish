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
using System.Runtime.Serialization;

namespace Catfish.Core.Models
{
    [Serializable]
    public abstract class CFEntity : CFXmlModel
    {
        public int? EntityTypeId { get; set; }

        [IgnoreDataMember]
        public virtual CFEntityType EntityType { get; set; }

        protected static string AccessGroupXPath = "access/" + CFAccessGroup.TagName;
        protected static string MetadataSetXPath = "metadata/" + CFMetadataSet.TagName;

        public CFEntity()
        {
            Data.Add(new XElement("metadata"));
            Data.Add(new XElement("access"));

            if (this.GetType().Namespace != "System.Data.Entity.DynamicProxies") // This is done to avoid a massive performance hit when loading models from the database.
            {
                InitializeExternally(this);
            }
        }

        [NotMapped]
        [IgnoreDataMember]
        public List<CFMetadataSet> MetadataSets
        {
            get
            {
                return GetChildModels(MetadataSetXPath)
                    .Select(c => c as CFMetadataSet).ToList();
            }

            set
            {
                RemoveAllMetadataSets();
                InitMetadataSet(value);
            }

        }

        public CFAccessGroup GetAccessGroup(Guid guid)
        {
            return AccessGroups.Where(x => x.AccessGuid == guid).FirstOrDefault();
        }

        public CFAccessGroup GetOrCreateAccess(Guid guid)
        {
            CFAccessGroup accessGroup = GetAccessGroup(guid);

            if (accessGroup == null)
            {
                accessGroup = new CFAccessGroup();
                AccessGroups.Add(accessGroup);
            }

            return accessGroup;
        }

        public void SetAccess(Guid guid, AccessMode accessMode, bool isInherited = false)
        {            
            CFAccessGroup accessGroup = GetOrCreateAccess(guid);
            accessGroup.IsInherited = isInherited;
            accessGroup.AccessGuid = guid;
            accessGroup.AccessDefinition.AccessModes = accessMode;
            // XXX is this saved ?
        }

        [NotMapped]
        [IgnoreDataMember]
        public List<CFAccessGroup> AccessGroups
        {
            get
            {

                return GetChildModels(AccessGroupXPath)
                    .Select(c => c as CFAccessGroup).ToList();
            }

            set
            {
                RemoveAllElements(AccessGroupXPath);
                InitModels("access", value);
            }
        }
        [NotMapped]
        [IgnoreDataMember]
        public bool BlockInheritance
        {
            get
            {
                XElement access = GetImmediateChild("access");
                if (access != null)
                {
                    return GetAttribute("blockInheritance", access) == "true";
                }
                return false;
            }
            set
            {
                SetAttribute("blockInheritance", value, GetImmediateChild("access"));
            }
        }


        public void RemoveAllMetadataSets()
        {
            //Removing all children inside the metadata set element
            RemoveAllElements(MetadataSetXPath);
        }

        public void InitMetadataSet(IReadOnlyList<CFMetadataSet> src)
        {
            XElement metadata = GetImmediateChild("metadata");
            foreach (CFMetadataSet ms in src)
                metadata.Add(ms.Data);
        }

        private void InitModels(string element, IReadOnlyList<CFXmlModel> models)
        {
            XElement access = GetImmediateChild(element);
            foreach (CFAccessGroup model in models)
            {
                access.Add(model.Data);
            }
        }

        protected FormField GetMetadataSetField(string metadatasetGuid, string fieldName)
        {
            CFMetadataSet metadataSet = MetadataSets.Where(ms => ms.Guid == metadatasetGuid).FirstOrDefault();

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

                if (typeof(OptionsField).IsAssignableFrom(field.GetType()))
                {
                    return string.Join(", ", GetAttributeMappingOptionValues(name, lang));
                }

                return MultilingualHelper.Join(field.GetValues(), " / ", false);
            }

            return null;
        }

        public string[] GetAttributeMappingOptionValues(string name, string lang = null)
        {
            var mapping = EntityType.AttributeMappings.Where(m => m.Name == name).FirstOrDefault();
            if (mapping != null)
            {
                string msGuid = mapping.MetadataSet.Guid;
                string fieldName = mapping.FieldName;

                FormField field = GetMetadataSetField(msGuid, fieldName);

                if (field == null || !typeof(OptionsField).IsAssignableFrom(field.GetType()))
                {
                    return new string[] { string.Format("ERROR: INCORRECT {0} OPTIONS MAPPING FOUND FOR THIS ENTITY TYPE", mapping) };
                }

                OptionsField optionField = (OptionsField)field;
                IEnumerable<List<TextValue>> values = optionField.Options.Where(o => o.Selected).Select(o => o.Value);

                if (lang != null)
                {
                    return values.SelectMany(v => v).Where(v => v.LanguageCode == lang).Select(v => v.Value).ToArray();
                }

                return values.Select(v => MultilingualHelper.Join(v, " / ", false)).ToArray();
            }

            return new string[] { };
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
            CFEntityTypeAttributeMapping mapping = EntityType.AttributeMappings.Where(m => m.Name == name).FirstOrDefault();
            if (mapping == null)
                throw new Exception(string.Format("{0} mapping metadata set is not specified for this entity type", name));

            if (string.IsNullOrEmpty(mapping.FieldName))
                throw new Exception(string.Format("Field is not specified in the {0} Mapping of this entity type", name));

            CFMetadataSet metadataSet = MetadataSets.Where(ms => ms.Guid == mapping.MetadataSet.Guid).FirstOrDefault();
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

        public override void UpdateValues(CFXmlModel src)
        {
            if (src == this)
            {
                // Updating will delete the child content. Since it's the same, we will return without any changes.
                return;
            }

            base.UpdateValues(src);

            var src_item = src as CFEntity;

            foreach (CFMetadataSet ms in this.MetadataSets)
            {
                var src_ms = src_item.MetadataSets.Where(x => x.Guid == ms.Guid).FirstOrDefault();
                ms.UpdateValues(src_ms);
            }
        }

        [IgnoreDataMember]
        public override string Name
        {
            get
            {
                return GetName();
            }
        }

        [IgnoreDataMember]
        public override string Description
        {
            get
            {
                return GetDescription();
            }
        }

    }
}
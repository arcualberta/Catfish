using System;
using System.Collections;
using System.Collections.Generic;
using Catfish.Core.Models.Forms;
//using System.Web.Script.Serialization;
using Catfish.Core.Models.Attributes;
using System.Xml.Linq;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Catfish.Core.Helpers;
using Catfish.Core.Models.Access;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Catfish.Core.Models
{
    [Serializable]
    public abstract class CFEntity : CFXmlModel
    {
        public int? EntityTypeId { get; set; }

        [IgnoreDataMember]
        public virtual CFEntityType EntityType { get; set; }
        public virtual string Label => "Entity";


        protected static string AccessGroupXPath = "access/" + CFAccessGroup.TagName;
        protected static string MetadataSetXPath = "metadata/" + CFMetadataSet.TagName;

        public CFEntity()
        {
            Data.Add(new XElement("metadata"));
            Data.Add(new XElement("access"));
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
                List<CFAccessGroup> accessGroups = AccessGroups.ToList();
                accessGroups.Add(accessGroup);
                AccessGroups = accessGroups;
            }

            return accessGroup;
        }

        public void SetAccess(Guid guid, AccessMode accessMode, bool isInherited = false)
        {            
            // At this point accessGroup is already part of the AccessGroups list
            CFAccessGroup accessGroup = GetOrCreateAccess(guid);
            accessGroup.IsInherited = isInherited;
            accessGroup.AccessGuid = guid;
            accessGroup.AccessDefinition.AccessModes = accessMode;
        }

        [NotMapped]
        [IgnoreDataMember]
        public IReadOnlyList<CFAccessGroup> AccessGroups
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

        private string BuildSolrKey(string prefix, string metadatasetGuid, string fieldGuid, string type, string languageCode)
        {
            return BuildSolrKey(prefix, metadatasetGuid, fieldGuid, type) + "_" + languageCode;
        }

        private string BuildSolrKey(string prefix, string metadatasetGuid, string fieldGuid, string type)
        {
            string key = prefix + "_"
                + metadatasetGuid + "_"
                + fieldGuid + "_"
                + type;
            return key;
        }

        //private Dictionary<string, object> GetSolrValues(string prefix,
        //    string metadatasetGuid,
        //    string fieldGuid,
        //    TextValue value)
        //{
        //    Dictionary<string, object> result = new Dictionary<string, object>();
        //    string txtKey = buildSolrKey(prefix, metadatasetGuid, fieldGuid, "txt", value.LanguageCode);
        //    result[txtKey] = value.Value;
        //    MatchCollection matches = Regex.Matches(value.Value, @"^(?=.)([+-]?([0-9]*)(\.([0-9]+))?)$");
        //    if (matches.Count > 0)
        //    {
        //        Decimal decimalValue = Decimal.Parse(value.Value);
        //        string decimalkey = buildSolrKey(prefix, metadatasetGuid, fieldGuid, "d");
        //        string integerKey = buildSolrKey(prefix, metadatasetGuid, fieldGuid, "i");
        //        result[decimalkey] = decimalValue;
        //        result[integerKey] = (int)Decimal.Round(decimalValue);
        //    }
        //    return result;
        //}

        private string CleanGuid(string guid)
        {
            return guid.Replace("-", "_");
        }

        //private Dictionary<string, object> GetFieldValues(FormField field, string metadatasetGuid, string fieldGuid)
        //{
        //    Dictionary<string, object> values;
        //    Dictionary<string, object> result = new Dictionary<string, object>();
        //    if (typeof(OptionsField).IsAssignableFrom(field.GetType()))
        //    {
        //        // Check if the field has options
        //        OptionsField optionsField = (OptionsField)field;
        //        foreach (Option option in optionsField.Options)
        //        {
        //            if (option.Selected)
        //            {
        //                //metadatasetGuid;
        //                string optionGuid = CleanGuid(option.Guid);
        //                foreach (TextValue value in option.Value)
        //                {
        //                    values = GetSolrValues("option_value", metadatasetGuid, optionGuid, value);
        //                    values.ToList().ForEach(x => result[x.Key] = x.Value);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // if this is not an options field
        //        foreach (TextValue value in field.Values)
        //        {
        //            values = GetSolrValues("value", metadatasetGuid, fieldGuid, value);
        //            values.ToList().ForEach(x => result[x.Key] = x.Value);
        //        }
        //    }

        //    return result;
        //}

        private Dictionary<string, List<string>> GetAccessDictionary()
        {

            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            //AccessGroups.ForEach( x =>
            foreach (CFAccessGroup accessGroup in AccessGroups)
            {
                foreach (AccessMode mode in accessGroup.AccessDefinition.AccessModes.AsList())
                {
                    string key = $@"access_{(int)mode}_ss";
                    if (!result.ContainsKey(key))
                    {
                        result[key] = new List<string>();
                    }

                    //result[key].Add(x.Guid.ToString());
                    result[key].Add(accessGroup.AccessGuid.ToString());
                }

            }
            return result;
        }

        private Type Unproxy(Type type)
        {
            throw new NotImplementedException("UNTESTED CODE: Check the value of the input argument and verify that the following implementation works.");
            if (type.Namespace.ToLower().Contains("Proxies"))
            {
                return type.BaseType;
            }
            return type;
        }

        public virtual Dictionary<string, object> ToSolrDictionary()
        {
            // KR:.NETCORE
            //string modelType = System.Data.Entity.Core.Objects.ObjectContext.GetObjectType(GetType()).Name;
            string modelType = Unproxy(GetType()).Name;

            Dictionary<string, object> result = new Dictionary<string, object>
            {
                {"id", Guid},
                {"modeltype_s", modelType},
                {"entitytype_s", this.EntityType == null ? "" : this.EntityType.Name },
                {"created_dt", this.Created.ToUniversalTime() }
                //{"created_date", this.Created.ToString("yyyy-MM-ddTHH:mm:ssZ") }
            };

            // Add access elements for secure searches in solr
            Dictionary<string, List<string>> access = GetAccessDictionary();
            access.ToList().ForEach(x => result[x.Key] = x.Value);

            // Add all of the name mapping
            string name = GetName();
            if (!String.IsNullOrEmpty(name)) { 
                result.Add("name_mapping_s", name.ToLowerInvariant());
            }


            //foreach (CFMetadataSet metadataset in MetadataSets)
            //{
            //    string metadatasetGuid = CleanGuid(metadataset.Guid);
            //    foreach (FormField field in metadataset.Fields)
            //    {
            //        string fieldGuid = CleanGuid(field.Guid);
            //        foreach (TextValue name in field.GetNames(false))
            //        {
            //            Dictionary<string, object> names = GetSolrValues("name", metadatasetGuid, fieldGuid, name);
            //            names.ToList().ForEach(x => result[x.Key] = x.Value);
            //        }
            //        Dictionary<string, object> values = GetFieldValues(field, metadatasetGuid, fieldGuid);
            //        values.ToList().ForEach(x => result[x.Key] = x.Value);
            //    }
            //}

            GetDynamicEntries(ref result);


            return result;            
        }

        private void GetDynamicValues(
            ref Dictionary<string, object> result, 
            string keyFields, 
            FormField field) {            

            

            Regex numberRegex = new Regex(@"^(?=.)([+-]?([0-9]*)(\.([0-9]+))?)$");


            // add field Options Values

            if (typeof(OptionsField).IsAssignableFrom(field.GetType()))
            {
                AddOptions(ref result, keyFields, field as OptionsField);
            } else
            {
                // add field Values
                foreach (TextValue textValue in field.Values)
                {

                    // language 
                    AddTextValue(ref result, keyFields, textValue);

                    // numbers

                    // if value can be interpreted as number add decimal and
                    // integer values to solr
                    if (numberRegex.Matches(textValue.Value).Count > 0)
                    {
                        string integerKey = $@"value_{keyFields}_is";
                        string decimalKey = $@"value_{keyFields}_ds";
                        Decimal decimalValue = Decimal.Parse(textValue.Value);

                        if (!result.ContainsKey(decimalKey))
                        {
                            result[decimalKey] = new List<decimal>();
                        }
                        if (!result.ContainsKey(integerKey))
                        {
                            result[integerKey] = new List<int>();
                        }

                        ((List<decimal>)result[decimalKey]).Add(decimalValue);
                        ((List<int>)result[integerKey]).Add((int)Decimal.Round(decimalValue));
                    }
                }
            }                    
        }

        /// <summary>
        /// Loop through options in OptionField and add selected Options to 
        /// result reference
        /// </summary>
        /// <param name="result">Reference to output value</param>
        /// <param name="keyFields">String concatenating metadataset guid and field guid</param>
        /// <param name="optionsField">Current working OptionField</param>
        private void AddOptions(ref Dictionary<string, object> result, 
            string keyFields, 
            OptionsField optionsField)
        {            

            foreach (Option option in optionsField.Options)
            {
                if(option.Selected)
                {
                    foreach(TextValue textValue in option.Value)
                    {
                        AddTextValue(ref result, keyFields, textValue);
                    }
                }
            }
        }

        /// <summary>
        /// Add the TextValue value creating the key value it it does not exists
        /// on result refrence parameter. The key generated includes the mapped
        /// metadataset guid and field guid pair as well as the language code.
        /// </summary>
        /// <param name="result">Reference to output value</param>
        /// <param name="keyFields">String concatenating metadataset guid and field guid</param>
        /// <param name="textValue">Current working TextValue</param>
        private void AddTextValue(ref Dictionary<string, object> result, 
            string keyFields, 
            TextValue textValue)
        {
            //if (!String.IsNullOrEmpty(textValue.Value))
            //{
                string key = $@"value_{keyFields}_txts_{textValue.LanguageCode}";
                if (!result.ContainsKey(key))
                {
                    result[key] = new List<string>();
                }
                ((List<string>)result[key]).Add(textValue.Value);
            //}            
        }

        private void GetDynamicEntries(ref Dictionary<string, object> result)
        {            
            //Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (CFMetadataSet metadataset in MetadataSets)
            {
                string metadatasetGuid = CleanGuid(metadataset.Guid);

                foreach (FormField field in metadataset.Fields)
                {
                    string keyFields = CleanGuid(metadataset.Guid + "_" + field.Guid);

                    // name
                    foreach (TextValue textValue in field.GetNames(false))
                    {
                        string key = $@"name_{keyFields}_txt_{textValue.LanguageCode}";
                        result[key] = textValue.Value;
                    }
                    // values
                    GetDynamicValues(ref result, keyFields, field);              
                }
            }        
        }



        //    MatchCollection matches = Regex.Matches(value.Value, @"^(?=.)([+-]?([0-9]*)(\.([0-9]+))?)$");
        //        if (matches.Count > 0)
        //        {
        //            Decimal decimalValue = Decimal.Parse(value.Value);
        //    string decimalkey = buildSolrKey(prefix, metadatasetGuid, fieldGuid, "d");
        //    string integerKey = buildSolrKey(prefix, metadatasetGuid, fieldGuid, "i");
        //    result[decimalkey] = decimalValue;
        //            result[integerKey] = (int) Decimal.Round(decimalValue);
        //}

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
            if(EntityType == null)
            {
                return null;
            }

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
               
                return MultilingualHelper.Join(field.GetValues(true, lang), " / ", false);
               
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

        protected void SetAttributeMappingValue(string name, string val, string lang = null, bool removePrevious=false)
        {
            CFEntityTypeAttributeMapping mapping = EntityType.AttributeMappings.Where(m => m.Name == name).FirstOrDefault();
            if (mapping == null)
                throw new Exception(string.Format("{0} mapping metadata set is not specified for this entity type", name));

            if (string.IsNullOrEmpty(mapping.FieldName))
                throw new Exception(string.Format("Field is not specified in the {0} Mapping of this entity type", name));

            CFMetadataSet metadataSet = MetadataSets.Where(ms => ms.Guid == mapping.MetadataSet.Guid).FirstOrDefault();
            metadataSet.SetFieldValue(mapping.FieldName, val, lang, removePrevious);
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
            SetAttributeMappingValue("Name Mapping", val, lang, true);
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
            SetAttributeMappingValue("Description Mapping", val, lang, true);
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
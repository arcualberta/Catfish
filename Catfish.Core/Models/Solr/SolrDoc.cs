using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;
using Newtonsoft.Json;

namespace Catfish.Core.Models.Solr
{
    public class SolrDoc
    {
        private XElement _root = new XElement("doc");
        public XElement Root => _root;

        public SolrDoc()
        {
        }
        public SolrDoc(Entity src, bool indexFieldNames)
        {
            AddId(src.Id);
            AddField("status_s", src.StatusId);
            AddField("group_s", src.GroupId);
            AddField("template_s", src.TemplateId);
            AddField("collection_s", src.PrimaryCollectionId);
            AddField("doc_type_ss", typeof(Item).IsAssignableFrom(src.GetType()) ? "item" : "entity");
            AddField("created_dt", src.Created);
            AddField("updated_dt", src.Updated);

            foreach (var child in src.MetadataSets)
                AddContainerFields("metadata", child, indexFieldNames);

            foreach (var child in src.DataContainer)
                AddContainerFields("data", child, indexFieldNames);

            IndexAggregatedDataFields(src);
        }

        protected void AddContainerFields(string containerPrefix, FieldContainer container, bool indexFieldNames)
        {
            foreach(var field in container.Fields)
            {
                //Backword compatibility fix: new items use MedataSet.TemplateId as the container ID part of the field name. However, this TemplateId
                //was introduced recently and the items created prior to introducing this TemplateId uses MetadataSet.Id as the container ID. Therefore,
                //in the statement below, we take the TemplateId as the container ID if it's defined but use the actual container's ID if the TemplateId
                //is not defined. 
                Guid? containerId = container.TemplateId != null ? container.TemplateId : container.Id;

                string solrFieldName = string.Format("{0}_{1}_{2}", containerPrefix, containerId, field.Id);
                if (typeof(TextField).IsAssignableFrom(field.GetType()))
                {
                    solrFieldName += "_ts";
                    foreach (var val in (field as TextField).Values)
                        foreach (var txt in val.Values.Where(t => !string.IsNullOrEmpty(t.Value)))
                            AddField(solrFieldName, txt.Value);
                }
                else if (typeof(OptionsField).IsAssignableFrom(field.GetType()))
                {
                    solrFieldName += "_ts";
                    foreach (var option in (field as OptionsField).Options.Where(op => op.Selected))
                        foreach (var txt in option.OptionText.Values.Where(t => !string.IsNullOrEmpty(t.Value)))
                            AddField(solrFieldName, txt.Value);
                }
                else if (typeof(IntegerField).IsAssignableFrom(field.GetType()))
                {
                    solrFieldName += "_is";
                    foreach (var txt in (field as IntegerField).Values.Where(txt => !string.IsNullOrEmpty(txt.Value)))
                        AddField(solrFieldName, int.Parse(txt.Value));
                }
                else if (typeof(DecimalField).IsAssignableFrom(field.GetType()))
                {
                    solrFieldName += "_ds";
                    foreach (var txt in (field as DecimalField).Values.Where(txt => !string.IsNullOrEmpty(txt.Value)))
                        AddField(solrFieldName, decimal.Parse(txt.Value));
                }
                else if (typeof(DateField).IsAssignableFrom(field.GetType()))
                {
                    solrFieldName += "_dts";
                    foreach (var txt in (field as DateField).Values.Where(txt => !string.IsNullOrEmpty(txt.Value)))
                        AddField(solrFieldName, DateTime.Parse(txt.Value));
                }
                else if (typeof(MonolingualTextField).IsAssignableFrom(field.GetType()))
                {
                    solrFieldName += "_ss";
                    foreach (var txt in (field as MonolingualTextField).Values.Where(txt => !string.IsNullOrEmpty(txt.Value)))
                        AddField(solrFieldName, txt.Value);
                }
                else if (typeof(FieldContainerReference).IsAssignableFrom(field.GetType()))
                {
                    solrFieldName += "_ss";
                    var refField = field as FieldContainerReference;
                    var refType = refField.RefType == FieldContainerReference.eRefType.metadata ? "metadata" : "data";
                    var val = string.Format("ref://{0}_{1}_", refType, refField.RefId);
                    AddField(solrFieldName, val);
                }

                //Adding the name of the field to the index.
                if (indexFieldNames)
                {
                    string solrNameFieldName = string.Format("cf-fn_{0}_s", solrFieldName);
                    AddField(solrNameFieldName, field.Name.GetConcatenatedContent(" / "));
                }

            }
        }

        protected void IndexAggregatedDataFields(Entity src)
        {
            MetadataSet aggregator = src.Template.GetFieldAggregatorMetadataSet();
            if (aggregator != null)
            {
                foreach (AggregateField field in aggregator.Fields)
                {
                    string solrFieldName = field.GetName();
                    if(field.ContentType == AggregateField.eContetType.text)
                        solrFieldName = solrFieldName  + "_ts";
                    else if (field.ContentType == AggregateField.eContetType.str)
                        solrFieldName = solrFieldName + "_ss";

                    List<string> values = new List<string>();
                    foreach (var fieldReference in field.Sources)
                    {
                        FieldContainer container;
                        switch (fieldReference.SourceType)
                        {
                            case Contents.Workflow.FieldReference.eSourceType.Data:
                                container = src.DataContainer.FirstOrDefault(dc => dc.TemplateId == fieldReference.FieldContainerId);
                                break;
                            case Contents.Workflow.FieldReference.eSourceType.Metadata:
                                container = src.MetadataSets.FirstOrDefault(dc => dc.TemplateId == fieldReference.FieldContainerId);
                                break;
                            default:
                                throw new Exception(String.Format("SolrDoc.IndexAggregatedDataFields: Unknown Source Type '{0}'", fieldReference.SourceType.ToString()));
                        }

                        if(container != null)
                        {
                            var vals = GetFieldValueStrings(container.Fields.FirstOrDefault(f => f.Id == fieldReference.FieldId));

                            if (!string.IsNullOrEmpty(fieldReference.ValueDelimiter))
                                vals = vals.SelectMany(val => val.Split(fieldReference.ValueDelimiter, StringSplitOptions.RemoveEmptyEntries))
                                    .Select(str => str.Trim())
                                    .Where(str => !string.IsNullOrEmpty(str))
                                    .ToList();

                            values.AddRange(vals.Where(v => !string.IsNullOrEmpty(v)));
                        }
                    }

                    //Converting each entry in values so that the entry will starts with an upper-case letter and will
                    //contain all other letters in lower case and then selecting the unique set of values
                    values = values.Select(v => v[0].ToString().ToUpper() + v.Substring(1).ToLower())
                        .Distinct()
                        .ToList();

                    foreach(var val in values)
                        AddField(solrFieldName, val);
                }
            }
        }

        protected List<string> GetFieldValueStrings(BaseField field)
        {
            if(field is TextField)
            {
                return (field as TextField).Values.SelectMany(val => val.Values).Select(txt => txt.Value).ToList();
            }
            else if(field is MonolingualTextField)
            {
                return (field as MonolingualTextField).Values.Select(txt => txt.Value).ToList();
            }
            else if (field is OptionsField)
            {
                return (field as OptionsField).Options.Where(opt => opt.Selected).SelectMany(opt => opt.OptionText.Values).Select(txt => txt.Value).ToList();

            }

            return new List<string>();
        }

        public override string ToString()
        {
            return _root == null ? null : _root.ToString();
        }

        public void AddField(TableField src)
        {
        }

        public void AddField(CompositeField src)
        {
        }

        public void AddId(Guid id)
        {
            _root.Add(NewField("id", id.ToString()));
        }

        public void AddField(string name, object val)
        {
            _root.Add(NewField(name, val));
        }

        protected XElement NewField(string name, object value = null)
        {
            XElement field = new XElement("field");
            field.SetAttributeValue("name", name);
            if (value != null)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
                string jsonString = JsonConvert.SerializeObject(value, settings);
                field.SetValue(value);
            }
            return field;
        }
    }
}

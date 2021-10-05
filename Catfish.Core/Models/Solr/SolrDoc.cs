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
        public SolrDoc(Entity src)
        {
            AddId(src.Id);
            AddField("status_s", src.StatusId);
            AddField("template_s", src.TemplateId);
            AddField("collection_s", src.PrimaryCollectionId);
            AddField("doc_type_ss", typeof(Item).IsAssignableFrom(src.GetType()) ? "item" : "entity");

            foreach (var child in src.MetadataSets)
                AddContainerFields("metadata", child);

            foreach (var child in src.DataContainer)
                AddContainerFields("data", child);
        }

        protected void AddContainerFields(string containerPrefix, FieldContainer container)
        {
            foreach(var field in container.Fields)
            {
                string solrFieldName = string.Format("{0}_{1}_{2}", containerPrefix, container.TemplateId, field.Id);
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

            }
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

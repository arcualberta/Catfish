using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Fields;

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

            XElement metadataSetContainer = NewField("metadata-set-container");
            _root.Add(metadataSetContainer);
            foreach(var child in src.DataContainer)
                metadataSetContainer.Add(new SolrDoc(child).Root);

            XElement dataContainer = NewField("data-container");
            _root.Add(dataContainer);
            foreach (var child in src.MetadataSets)
                dataContainer.Add(new SolrDoc(child).Root);
        }

        public SolrDoc(FieldContainerBase src)
        {
            AddId(src.Id);

            XElement fieldContainer = NewField("data-container");
            _root.Add(fieldContainer);
            foreach (var field in src.Fields)
                fieldContainer.Add(new SolrDoc(field).Root);
        }

        public SolrDoc(BaseField src)
        {
            bool status = false;

            if (typeof(TextField).IsAssignableFrom(src.GetType()))
                status = Initialize(src as TextField);
            else if (typeof(OptionsField).IsAssignableFrom(src.GetType()))
                status = Initialize(src as OptionsField);
            else if (typeof(IntegerField).IsAssignableFrom(src.GetType()))
                status = Initialize(src as IntegerField);
            else if (typeof(DecimalField).IsAssignableFrom(src.GetType()))
                status = Initialize(src as DecimalField);
            else if (typeof(DateField).IsAssignableFrom(src.GetType()))
                status = Initialize(src as DateField);
            else if (typeof(MonolingualTextField).IsAssignableFrom(src.GetType()))
                status = Initialize(src as MonolingualTextField);
            else if (typeof(TableField).IsAssignableFrom(src.GetType()))
                status = Initialize(src as TableField);
            else if (typeof(CompositeField).IsAssignableFrom(src.GetType()))
                status = Initialize(src as CompositeField);

            if (status)
            {
                AddId(src.Id);

            }

        }

        public override string ToString()
        {
            return _root == null ? null : _root.ToString();
        }

        public bool Initialize(TextField src)
        {
            return true;
        }

        public bool Initialize(OptionsField src)
        {
            return true;
        }

        public bool Initialize(IntegerField src)
        {
            return true;
        }

        public bool Initialize(DecimalField src)
        {
            return true;
        }

        public bool Initialize(DateField src)
        {
            return true;
        }

        public bool Initialize(MonolingualTextField src)
        {
            return true;
        }

        public bool Initialize(TableField src)
        {
            return true;
        }

        public bool Initialize(CompositeField src)
        {
            return true;
        }

        public void AddId(Guid id)
        {
            _root.Add(NewField("id", id.ToString()));
        }

        protected XElement NewField(string name, string value = null)
        {
            XElement field = new XElement("field");
            field.SetAttributeValue("name", name);
            if (!string.IsNullOrEmpty(value))
                field.SetValue(value);
            return field;
        }


    }
}

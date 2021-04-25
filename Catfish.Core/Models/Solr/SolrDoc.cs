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

            XElement metadataSetContainer = NewField("metadata-set-container");
            _root.Add(metadataSetContainer);
            foreach(var child in src.MetadataSets)
                metadataSetContainer.Add(new SolrDoc(child).Root);

            XElement dataContainer = NewField("data-container");
            _root.Add(dataContainer);
            foreach (var child in src.DataContainer)
                dataContainer.Add(new SolrDoc(child).Root);
        }

        public SolrDoc(FieldContainerBase src)
        {
            AddId(src.Id);

            ////XElement fieldContainer = NewField("data-container");
            ////_root.Add(fieldContainer);
            ////foreach (var field in src.Fields)
            ////    fieldContainer.Add(new SolrDoc(field).Root);
            
            foreach (var field in src.Fields)
            {
                if (typeof(TextField).IsAssignableFrom(field.GetType()))
                {
                    var firstValueSet = (field as TextField).Values.FirstOrDefault();
                    if (firstValueSet != null)
                    {
                        var languages = firstValueSet.Values.Select(v => v.Language).ToList();
                        
                        foreach (var lang in languages)
                            AddField(string.Format("{0}_{1}_ss", field.Id, lang), (field as TextField).GetStrValues(lang));
                    }
                }
                else if (typeof(OptionsField).IsAssignableFrom(field.GetType()))
                    AddField(field.Id +"_ss", (field as OptionsField).GetSelectedOptionTexts());
            }

        }

        public SolrDoc(BaseField src)
        {
            bool status = true;

            if (typeof(TextField).IsAssignableFrom(src.GetType())) { }
                //AddField(src as TextField);
            else if (typeof(OptionsField).IsAssignableFrom(src.GetType()))
                AddField("value_ss", (src as OptionsField).GetSelectedOptionTexts());
            else if (typeof(IntegerField).IsAssignableFrom(src.GetType()))
                AddField("value_ss", (src as IntegerField).GetValues());
            //else if (typeof(DecimalField).IsAssignableFrom(src.GetType()))
            //    AddField(src.Id+"_ds", (src as DecimalField).GetValues());
            //else if (typeof(DateField).IsAssignableFrom(src.GetType()))
            //    AddField(src.Id+"_dts", (src as DateField).GetValues());
            //else if (typeof(MonolingualTextField).IsAssignableFrom(src.GetType()))
            //    AddField(src.Id+"_ss", (src as MonolingualTextField).GetValues());
            else if (typeof(TableField).IsAssignableFrom(src.GetType()))
                AddField(src as TableField);
            else if (typeof(CompositeField).IsAssignableFrom(src.GetType()))
                AddField(src as CompositeField);
            else
                status = false;

            if (status)
            {
                AddId(src.Id);
            }
        }

        public override string ToString()
        {
            return _root == null ? null : _root.ToString();
        }

        public void AddField(TextField src)
        {
            var firstValueSet = src.Values.FirstOrDefault();
            if (firstValueSet == null)
                return;

            var languages = firstValueSet.Values.Select(v => v.Language).ToList();
            foreach(var lang in languages)
                AddField(string.Format("{0}_{1}_ss", src.Id, lang), src.GetStrValues(lang));
        }

        ////public void AddField(OptionsField src)
        ////{
        ////    AddField(src.Id + "_ss", src.GetSelectedOptionTexts());
        ////}

        ////public void AddField(IntegerField src)
        ////{
        ////    var vals = src.GetValues();
        ////    if (src.AllowMultipleValues)
        ////        AddField(src.Id + "_is", vals);
        ////    else if (vals.Length > 0)
        ////        AddField(src.Id + "_i", vals[0]);
        ////}

        ////public void AddField(DecimalField src)
        ////{
        ////    var vals = src.GetValues();
        ////    if (src.AllowMultipleValues)
        ////        AddField(src.Id + "_ds", vals);
        ////    else if (vals.Length > 0)
        ////        AddField(src.Id + "_d", vals[0]);
        ////}

        ////public void AddField(DateField src)
        ////{
        ////    var vals = src.GetValues();
        ////    if (src.AllowMultipleValues)
        ////        AddField(src.Id + "_dts", vals);
        ////    else if (vals.Length > 0)
        ////        AddField(src.Id + "_dt", vals[0]);
        ////}

        ////public void AddField(MonolingualTextField src)
        ////{
        ////    var vals = src.GetValues();
        ////    if (src.AllowMultipleValues)
        ////        AddField(src.Id + "_ss", vals);
        ////    else if (vals.Length > 0)
        ////        AddField(src.Id + "_s", vals[0]);
        ////}

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
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
            string jsonString = JsonConvert.SerializeObject(val, settings);
            _root.Add(NewField(name, jsonString));
        }

        ////public void AddField(string name, int[] values)
        ////{
        ////    JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
        ////    string jsonString = JsonConvert.SerializeObject(values, settings);
        ////    _root.Add(NewField(name, jsonString));
        ////}
        ////public void AddField(string name, int value)
        ////{
        ////    JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };
        ////    string jsonString = JsonConvert.SerializeObject(value, settings);
        ////    _root.Add(NewField(name, jsonString));
        ////}

        ////protected XElement NewField(string name, string value = null)
        ////{
        ////    XElement field = new XElement("field");
        ////    field.SetAttributeValue("name", name);
        ////    if (!string.IsNullOrEmpty(value))
        ////        field.SetValue(value);
        ////    return field;
        ////}

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

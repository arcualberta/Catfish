using System.Xml.Linq;

namespace Catfish.API.Repository.Solr
{
    public class SolrDoc
    {
        private XElement _root = new XElement("doc");
        public XElement Root => _root;

        public SolrDoc()
        {
        }
        public SolrDoc(EntityData src,List<FormTemplate> forms, bool indexFieldNames)
        {
            AddId(src.Id);
            AddField("status_s", src.State.ToString());
            AddField("template_s", src.TemplateId);
            AddField("doc_type_ss", src.EntityType.ToString());
            AddField("created_dt", src.Created);
            AddField("updated_dt", src.Updated);
            AddField("title_s", src.Title);
            AddField("description_s", src.Description);

            if ((src.Data?.Count) >= 1)
            {
                foreach(var frmData in src.Data)
                {

                    //AddField("form_id_s", frmData.FormId);
                    string solrFormState = string.Format("form_{0}_state_s", frmData.FormId);
                    AddField(solrFormState, frmData.State.ToString());
                    FormTemplate form = forms.FirstOrDefault(f => f.Id == frmData.FormId);
                    var fieldDataList = frmData.FieldData;
                    if (fieldDataList != null)
                    {
                        foreach (FieldData fd in fieldDataList)
                        {
                            string solrFieldName = string.Format("field_{0}_{1}_", frmData.FormId, fd.FieldId);
                            var field = form?.Fields?.FirstOrDefault(fld => fld.Id == fd.FieldId);
                            if (fd.MonolingualTextValues?.Length > 0)
                            {
                                if (field!.Type == FieldType.Email)
                                    solrFieldName += "ss";
                                else if (field.Type == FieldType.Date || field?.Type == FieldType.DateTime)
                                    solrFieldName += "dts"; 
                                else if (field!.Type == FieldType.Integer)
                                    solrFieldName += "is";
                                else if (field.Type == FieldType.Decimal)
                                    solrFieldName += "ds";
                                //ISURU: The following field types are always multilingual, so there is no need to handle them in monolingual case.
                                //else if (field.Type == FieldType.ShortAnswer || field.Type == FieldType.Paragraph || field.Type == FieldType.RichText)
                                //    solrFieldName += "ts";

                                foreach (Text text in fd.MonolingualTextValues)
                                {
                                    if (text != null)
                                        AddField(solrFieldName, text.Value);
                                }     
                            }
                            else if (fd.MultilingualTextValues?.Length > 0)
                            {
                                solrFieldName += "ts";
                                foreach (TextCollection tc in fd.MultilingualTextValues)
                                {
                                    foreach(Text text in tc.Values)
                                        AddField(solrFieldName, text.Value);
                                }
                            }
                            else if(fd.SelectedOptionIds?.Length > 0)
                            {
                                //TODO: 
                                //Field type  should be _ss (i.e. multiple strings)
                                //We should index the labels of the selected options.
                                solrFieldName += "ss";
                                
                                foreach (var optId in fd.SelectedOptionIds)
                                {
                                    //ISURU: We can use linq to avoid nested loop
                                    //foreach (var option in field!.options)
                                    //{
                                    //    //if(option.OptionText.Id == optId)
                                    //    if(option.Id == optId)
                                    //    {
                                    //        foreach(var value in option.OptionText.Values)
                                    //            if(!string.IsNullOrEmpty(value.Value))
                                    //                AddField(solrFieldName, value.Value);
                                    //    }
                                    //}

                                    var selectedOption = field!.options!.FirstOrDefault(opt => opt.OptionText.Id == optId);
                                    var selectedOptionLabelValues = selectedOption!.OptionText.Values.Select(txt => txt.Value);
                                    AddField(solrFieldName, selectedOptionLabelValues.ToArray());

                                    //If this selected option contains an extended values, we will index them also here
                                    var extVals = fd.ExtendedOptionValues?.FirstOrDefault(ext => ext.OptionId == optId)?.Values;
                                    if(extVals?.Length > 0)
                                        AddField(solrFieldName, extVals); 
                                }

                                //if (fd.ExtendedOptionValues?.Length > 0)
                                //{
                                //    foreach (ExtendedOptionValue extVal in fd.ExtendedOptionValues)
                                //        foreach (string val in extVal.Values)
                                //            AddField(solrFieldName, val);
                                //}
                            }
                            
                            if(fd.CustomOptionValues?.Length > 0)
                            {
                                if (!solrFieldName.EndsWith("ss"))
                                    solrFieldName += "ss";
                                AddField(solrFieldName, fd.CustomOptionValues);
                            }
                        }
                    }
                }
            }

            IndexAggregatedDataFields(src);
        }

         
        protected void IndexAggregatedDataFields(EntityData src)
        {
            /*
            MetadataSet aggregator = src.Template.GetFieldAggregatorMetadataSet();
            if (aggregator != null)
            {
                foreach (AggregateField field in aggregator.Fields)
                {
                    string solrFieldName = field.GetName();
                    if (field.ContentType == AggregateField.eContetType.text)
                        solrFieldName = solrFieldName + "_ts";
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

                        if (container != null)
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

                    foreach (var val in values)
                        AddField(solrFieldName, val);
                }
            }
            */
        }
        
        public void AddId(Guid id)
        {
            _root.Add(NewField("id", id.ToString()));
        }

        public void AddId(string id)
        {
            _root.Add(NewField("id", id));
        }

        public void AddId(int id)
        {
            _root.Add(NewField("id", id));
        }

        public void AddField(string name, int? val)
        {
            if (val.HasValue)
                _root.Add(NewField(name, val));
        }

        public void AddField(string name, int[]? values)
        {
            if (values != null)
            {
                foreach (int val in values)
                    _root.Add(NewField(name, val));
            }
        }

        public void AddField(string name, decimal? val)
        {
            if (val.HasValue)
                _root.Add(NewField(name, val.Value));
        }

        public void AddField(string name, decimal[]? values)
        {
            if (values != null)
            {
                foreach (decimal val in values)
                    _root.Add(NewField(name, val));
            }
        }

        public void AddField(string name, string? val)
        {
            if (!string.IsNullOrEmpty(val))
                _root.Add(NewField(name, val));
        }

        public void AddField(string name, string[]? values)
        {
            if (values != null)
            {
                foreach (string val in values)
                    _root.Add(NewField(name, val));
            }
        }

        public void AddField(string name, DateTime? val)
        {
            if (val.HasValue)
                _root.Add(NewField(name, val));
        }

        public void AddField(string name, DateTime[]? values)
        {
            if (values != null)
            {
                foreach (DateTime val in values)
                    _root.Add(NewField(name, val));
            }
        }


        public void AddField(string name, Guid? val)
        {
            if (val.HasValue)
                _root.Add(NewField(name, val));
        }

        //public void AddField(string name, object[] values)
        //{
        //    foreach (object val in values)
        //        _root.Add(NewField(name, val));
        //}

        protected XElement NewField(string name, object? value = null)
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

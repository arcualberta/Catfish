using System.Xml.Linq;

namespace Catfish.API.Repository.Solr
{
    public class SolrDoc
    {
        private readonly RepoDbContext _context;
        private XElement _root = new XElement("doc");
        public XElement Root => _root;

        public SolrDoc(RepoDbContext context)
        {
            _context = context;
        }
        public SolrDoc(EntityData src, bool indexFieldNames)
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
                    
                    AddField("form_id_s", frmData.FormId);
                    AddField("form_state_s", frmData.State.ToString());
                    FormTemplate form = _context!.Forms!.FirstOrDefault(f => f.Id == frmData.FormId);
                    var fieldDataList = frmData.FieldData;
                    if (fieldDataList != null)
                    {
                        foreach (FieldData fd in fieldDataList)
                        {
                            string solrFieldName = string.Format("field_{0}_{1}_", frmData.FormId, fd.FieldId);
                            var field = form!.Fields.FirstOrDefault(fld => fld.Id == fd.FieldId);
                            if (fd.MonolingualTextValues?.Length > 0)
                            {
                                if (field.Type == FieldType.Email)
                                    solrFieldName += "ss";
                                else if (field.Type == FieldType.Date || field?.Type == FieldType.DateTime)
                                    solrFieldName += "dts"; 
                                else if (field.Type == FieldType.Integer)
                                    solrFieldName += "is";
                                else if (field.Type == FieldType.Decimal)
                                    solrFieldName += "ds";
                                else if (field.Type == FieldType.ShortAnswer || field.Type == FieldType.Paragraph || field.Type == FieldType.RichText)
                                    solrFieldName += "ts";

                                foreach (Text text in fd.MonolingualTextValues)
                                {
                                    if (text != null)
                                        AddField(solrFieldName, text.Value);
                                }     
                            }

                            if (fd.MultilingualTextValues?.Length > 0)
                            {
                                solrFieldName += "ts";
                                foreach (TextCollection tc in fd.MultilingualTextValues)
                                {
                                    foreach(Text text in tc.Values)
                                        AddField(solrFieldName, text.Value);
                                }
                            }

                            if(fd.SelectedOptionIds?.Length > 0)
                            {
                                //TODO: 
                                //Field type  should be _ss (i.e. multiple strings)
                                //We should index the labels of the selected options.
                                solrFieldName += "ss";
                                foreach (var optId in fd.SelectedOptionIds)
                                    AddField(solrFieldName, optId.ToString());

                                if (fd.ExtendedOptionValues?.Length > 0)
                                {
                                    foreach (ExtendedOptionValue extVal in fd.ExtendedOptionValues)
                                        foreach (string val in extVal.Values)
                                            AddField(solrFieldName, val);
                                }
                            }
                            
                            if(fd.CustomOptionValues?.Length > 0)
                            {
                                if (!solrFieldName.EndsWith("ss"))
                                    solrFieldName += "ss";
                                foreach(string val in fd.CustomOptionValues)
                                    AddField(solrFieldName, val);
                            }
                        }
                    }
                }
            }

            IndexAggregatedDataFields(src);
        }

      

     /*   protected void AddContainerFields(string containerPrefix, FieldContainer container, bool indexFieldNames)
        {
            //Backword compatibility fix: new items use MedataSet.TemplateId as the container ID part of the field name. However, this TemplateId
            //was introduced recently and the items created prior to introducing this TemplateId uses MetadataSet.Id as the container ID. Therefore,
            //in the statement below, we take the TemplateId as the container ID if it's defined but use the actual container's ID if the TemplateId
            //is not defined. 
            Guid? containerId = container.TemplateId != null ? container.TemplateId : container.Id;
            string solrContainerNamePrefix = string.Format("{0}_{1}", containerPrefix, containerId);

            foreach (var field in container.Fields)
            {
                string solrFieldName = string.Format("{0}_{1}", solrContainerNamePrefix, field.Id);
                if (typeof(TextField).IsAssignableFrom(field.GetType()))
                {
                    solrFieldName += "_ts";
                    foreach (var val in (field as TextField).Values)
                        foreach (var txt in val.Values.Where(t => !string.IsNullOrEmpty(t.Value)))
                            AddField(solrFieldName, txt.Value);
                }
                else if (typeof(OptionsField).IsAssignableFrom(field.GetType()))
                {
                    solrFieldName += field.SolrFieldType.ToString();
                    foreach (var option in (field as OptionsField).Options.Where(op => op.Selected))
                    {
                        foreach (var txt in option.OptionText.Values.Where(t => !string.IsNullOrEmpty(t.Value)))
                            AddField(solrFieldName, txt.Value);

                        if (option.ExtendedOption && option.ExtendedValues?.Length > 0)
                            foreach (string val in option.ExtendedValues)
                                if (!string.IsNullOrEmpty(val))
                                    AddField(solrFieldName, val);
                    }
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
                else if (typeof(AttachmentField).IsAssignableFrom(field.GetType()))
                {
                    var attachmentField = field as AttachmentField;
                    if (attachmentField.Files.Count == 0)
                        continue;

                    var fileIds = attachmentField.Files.Select(file => file.Id).ToArray();
                    foreach (var val in fileIds)
                        AddField(string.Format("{0}_id_ss", solrFieldName), val);

                    var originalFileNames = attachmentField.Files.Select(file => file.OriginalFileName).ToArray();
                    foreach (var val in originalFileNames)
                        AddField(string.Format("{0}_original_ss", solrFieldName), val);

                    var fileNames = attachmentField.Files.Select(file => file.FileName).ToArray();
                    foreach (var val in fileNames)
                        AddField(string.Format("{0}_filename_ss", solrFieldName), val);

                    var thumbnails = attachmentField.Files.Select(file => file.Thumbnail).ToArray();
                    foreach (var val in thumbnails)
                        AddField(string.Format("{0}_thumbnail_ss", solrFieldName), val);

                    var sizes = attachmentField.Files.Select(file => file.Size).ToArray();
                    foreach (var val in sizes)
                        AddField(string.Format("{0}_size_is", solrFieldName), val);

                    var createdTimestamps = attachmentField.Files.Select(file => file.Created).ToArray();
                    foreach (var val in createdTimestamps)
                        AddField(string.Format("{0}_created_dts", solrFieldName), val);

                    var contentTypes = attachmentField.Files.Select(file => file.ContentType).ToArray();
                    foreach (var val in contentTypes)
                        AddField(string.Format("{0}_content-type_ss", solrFieldName), val);
                }

                //Adding the name of the field to the index.
                if (indexFieldNames)
                {
                    string solrNameFieldName = string.Format("cf-fn_{0}_s", solrFieldName);
                    AddField(solrNameFieldName, field.Name.GetConcatenatedContent(" / "));
                }

            }
        }
     */
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
        /*
        protected List<string> GetFieldValueStrings(BaseField field)
        {
            if (field is TextField)
            {
                return (field as TextField).Values.SelectMany(val => val.Values).Select(txt => txt.Value).ToList();
            }
            else if (field is MonolingualTextField)
            {
                return (field as MonolingualTextField).Values.Select(txt => txt.Value).ToList();
            }
            else if (field is OptionsField)
            {
                OptionsField optionField = field as OptionsField;
                List<string> selectedOptionValues = optionField.Options.Where(opt => opt.Selected).SelectMany(opt => opt.OptionText.Values).Select(txt => txt.Value).ToList();
                var extendedOptionValues = optionField.Options.Where(opt => opt.Selected && opt.ExtendedOption).SelectMany(opt => opt.ExtendedValues);
                selectedOptionValues.AddRange(extendedOptionValues);

                return selectedOptionValues;
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
        */
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

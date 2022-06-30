using Catfish.Core.Authorization.Requirements;
using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Models.Contents.Reports;
using Catfish.Core.Models.Contents.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    public class EntityTemplate : Entity
    {
        public static readonly string FieldAggregatorMetadataSetInternalName = "FieldAggregator";
        public string TargetType { get; set; }
        public string TemplateName { get; set; }

        [Computed]
        public string Domain 
        {
            get 
            {
                if (Workflow == null)
                    return null;
                else                        
                {
                    if (Workflow.Actions.Where(a => a.Function == nameof(TemplateOperations.Instantiate)
                                                 && a.Access == GetAction.eAccess.Public).Any())
                    {
                        return "*";
                    }
                    else
                    {
                        var domains = Workflow.Actions.Where(a => a.Function == nameof(TemplateOperations.Instantiate))
                                                      .SelectMany(a => a.States.SelectMany(s => s.AuthorizedDomains))
                                                      .Select(d => d.Value)
                                                      .ToList();

                        return (domains.Count > 0)
                            ? string.Join(";", domains) + ";" //Needs to end by a semicolon beause we will use it when matching domains.
                            : null;
                    }
                }
            }
            set
            {

            }
        }

        public ICollection<Entity> Instances { get; set; }

        private Workflow mWorkflow;
        [NotMapped]
        public Workflow Workflow
        {
            get
            {
                if (mWorkflow == null)
                {
                    XmlModel xml = new XmlModel(Data);
                    mWorkflow = new Workflow(xml.GetElement(Workflow.TagName, true));
                }
                return mWorkflow;

            }
        }

        public EntityTemplate()
            : base()
        {
            ////Initialize(false);
        }

        public EntityTemplate(XElement data)
            : base(data)
        {
        }



        public override void Initialize(bool regenerateId)
        {
            base.Initialize(regenerateId);
            mWorkflow = null;
            ////InitializeWorkflow();
        }

        ////public void InitializeWorkflow()
        ////{
        ////    XElement workflowDef = Data.Element("workflow");
        ////    if (workflowDef != null)
        ////    {
        ////        Workflow = new Workflow(workflowDef);
        ////        //Workflow.Initialize(XmlModel.eGuidOption.Ignore);
        ////    }
        ////}


        public EmailTemplate GetEmailTemplate(string templateName, string lang, bool createIfNotExists)
        {
            MetadataSet ms = GetMetadataSet(templateName, lang, createIfNotExists, true);
            return ms == null ? null : new EmailTemplate(ms.Data);
        }

        public EntityTemplate Clone()
        {
            Type type = GetType();
            EntityTemplate model = Activator.CreateInstance(type) as EntityTemplate;
            model.Data = new XElement(Data);
            model.Initialize(true); //We are cloning an entity. We must force to regenerate a new Id for the clone.
            return model;
        }

        public T Instantiate<T>() where T:Entity
        {
            var type = typeof(T);
            T model = Activator.CreateInstance(type) as T;

            //The new instance is created from the this template, so set its template-id to the ID of this template
            model.TemplateId = Id;

            //We are cloning an entity. We must force to regenerate a new Id for the clone.
            model.Initialize(true);

            //Cloning non-template type metadata sets
            foreach (var ms in MetadataSets.Where(m => m.IsTemplate == false))
            {
                MetadataSet clone = new MetadataSet(new XElement(ms.Data));
                clone.TemplateId = ms.Id;
                clone.Id = Guid.NewGuid();
                model.MetadataSets.Add(clone);
            }

            //NOTE: we do not need to clone the data items from the template because the data items are
            //added to the instantiated entity when it's used later in the application. The entity template
            //contains the templates for data items but they do not carry data.

            return model;
        }

        public DataItem InstantiateDataItem(Guid templateDataItemId)
        {
            //Get the item template from the entity template
            DataItem itemTemplate = DataContainer.Where(it => it.Id == templateDataItemId).FirstOrDefault();
            if (itemTemplate == null)
                throw new Exception("DataItem template with ID = " + templateDataItemId + " not found in the EntityTemplate with ID = " + Id);

            //Create a clone of the item template as the data item
            DataItem dataItem = itemTemplate.Clone() as DataItem;

            //Initialize the clone and force to have its own ID
            dataItem.Initialize(XmlModel.eGuidOption.Regenerate);

            //Set the template ID as the ID of the template
            dataItem.TemplateId = itemTemplate.Id;

            return dataItem;
        }

        public DataItem InstantiateRootItem()
        {
            return InstantiateDataItem(GetRootDataItem(false).Id);
        }

        public T GetReport<T>(string reportName,Guid entityTemplateId, bool createIfNotExists) where T : BaseReport
        {
            BaseReport report = Reports
                .Where(rep => rep.Name == reportName)
                .FirstOrDefault();

            if (report == null && createIfNotExists)
            {
                var t = typeof(T);

                report = Activator.CreateInstance(t) as T;
                report.SetAttribute("name", reportName);
                report.SetAttribute("entity-template-id", entityTemplateId);
                Reports.Add(report);
            }
            return report as T;
        }

        public MetadataSet GetFieldAggregatorMetadataSet(bool createIfNotExist = false)
        {
            MetadataSet aggregatorMetadataSet = MetadataSets.FirstOrDefault(ms => ms.IsTemplate && ms.InternalName == "FieldAggregator");
            if (aggregatorMetadataSet == null && createIfNotExist)
            {
                aggregatorMetadataSet = new MetadataSet() 
                { 
                    InternalName = FieldAggregatorMetadataSetInternalName,
                    IsTemplate = true
                };
                MetadataSets.Add(aggregatorMetadataSet);
            }
            return aggregatorMetadataSet;
        }


    }
}

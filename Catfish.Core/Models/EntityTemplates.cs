using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Data;
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
        public string TargetType { get; set; }
        public string TemplateName { get; set; }

        [NotMapped]
        public Workflow Workflow { get; set; }

        public EntityTemplate()
        {
            Initialize(false);
        }

        public override void Initialize(bool regenerateId)
        {
            base.Initialize(regenerateId);

            XElement workflowDef = Data.Element("workflow");
            if(workflowDef != null)
            {
                Workflow = new Workflow(workflowDef);
            }
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
                model.MetadataSets.Add(new MetadataSet(new XElement(ms.Data)));

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
    }

    public class ItemTemplate : EntityTemplate { }
    public class CollectionTemplate : EntityTemplate { }

}

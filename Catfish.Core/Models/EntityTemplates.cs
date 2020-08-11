using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Workflow;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

            XElement metadataSetContainer = Data.Element(Entity.MetadataSetsRootTag);
            model.ReplaceMetadataSetContainer(new XElement(metadataSetContainer));

            XElement dataSetContainer = Data.Element(Entity.DataContainerRootTag);
            model.ReplaceDataSetContainer(new XElement(dataSetContainer));

            model.Initialize(true); //We are cloning an entity. We must force to regenerate a new Id for the clone.
            model.TemplateId = Id;

            return model;
        }
    }

    public class ItemTemplate : EntityTemplate { }
    public class CollectionTemplate : EntityTemplate { }

}

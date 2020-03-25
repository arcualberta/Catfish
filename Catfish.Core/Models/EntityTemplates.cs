using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    public class EntityTemplate : Entity
    {
        public string TypeName { get; set; }

        public EntityTemplate()
        {
            Initialize(false);
        }

        public T Clone<T>() where T:Entity
        {
            var type = typeof(T);
            T model = Activator.CreateInstance(type) as T;
            model.Data = new XElement(Data);
            model.Initialize(true); //We are cloning an entity. We must force to regenerate a new Id for the clone.
            return model;
        }
    }

    public class ItemTemplate : EntityTemplate { }
    public class CollectionTemplate : EntityTemplate { }

}

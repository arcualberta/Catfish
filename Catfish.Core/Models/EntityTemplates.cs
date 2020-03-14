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
            Initialize();
        }

        public T Clone<T>() where T:Entity
        {
            var type = typeof(T);
            T model = Activator.CreateInstance(type) as T;
            model.Data = new XElement(Data);
            model.Initialize();
            return model;
        }
    }

    public class ItemTemplate : EntityTemplate { }
    public class CollectionTemplate : EntityTemplate { }

}

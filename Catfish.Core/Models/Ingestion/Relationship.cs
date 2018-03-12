using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Ingestion
{
    public class Relationship
    {
        public string ParentRef { get; set; }

        public string ChildRef { get; set; }

        public bool IsMember { get; set; }

        public bool IsRelated { get; set; }

        public Relationship()
        {
            IsMember = true;
            IsRelated = false;
            ParentRef = null;
            ChildRef = null;
        }

        public XElement Serialize()
        {
            XElement result = new XElement("relationship");
            result.SetAttributeValue("is-member", IsMember);
            result.SetAttributeValue("is-related", IsRelated);

            XElement parent = new XElement("parent");
            parent.SetAttributeValue("ref", ParentRef);
            result.Add(parent);

            XElement child = new XElement("child");
            child.SetAttributeValue("ref", ChildRef);
            result.Add(child);

            return result;
        }

        public Relationship Deserialize(XElement relationship)
        {
            if(relationship.Name != "relationship")
            {
                throw new FormatException("Invalid XML relationship element.");
            }

            foreach(XAttribute attribute in relationship.Attributes())
            {
                string name = attribute.Name.LocalName;
                if (name == "is-member")
                {
                    IsMember = bool.Parse(attribute.Value);
                }
                else if (name == "is-related")
                {
                    IsRelated = bool.Parse(attribute.Value);
                }
            }

            foreach (XElement element in relationship.Elements())
            {
                string name = element.Name.LocalName;
                if (name == "parent")
                {
                    ParentRef = element.Attribute("ref").Value;
                }
                else if (name == "child")
                {
                    ChildRef = element.Attribute("ref").Value;
                }
            }

            return this;
        }
    }
}

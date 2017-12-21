using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    [NotMapped]
    public class AuditChangeLog
    {
        public string Target { get; set; }

        public string Description { get; set; }

       public AuditChangeLog(string target, string description)
        {
            Target = target;
            Description = description;
        }
/*
         public static string TagName { get { return "log"; } }


        public AuditChangeLog(XElement ele)
        {
            Target = ele.Attribute("tartget").Value;
            Description = ele.Value;
        }

        public XElement ToXml()
        {
            XElement element = new XElement(TagName);
            element.SetAttributeValue("target", Target);
            element.Value = Description;
            return element;
        }
        */
    }
}

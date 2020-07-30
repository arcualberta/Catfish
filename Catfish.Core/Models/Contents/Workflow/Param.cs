using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class Param : XmlModel
    {
        public string TemplateId { get; set; }
        public Param(XElement data)
            : base(data)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class Param : XmlModel
    {
        public static readonly string TagName = "param";
        public static readonly string TemlateAtt = "template-id";
        public Guid TemplateId
        {
            get => Guid.Parse(Data.Attribute("template-id").Value);
            set => SetAttribute("template-id", value);
        }
        public Param(XElement data)
            : base(data)
        {

        }
        public Param()
            : base(new XElement(TagName))
        {

        }
    }
}

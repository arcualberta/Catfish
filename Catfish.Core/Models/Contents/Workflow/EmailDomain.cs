using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class EmailDomain : XmlModel
    {
        public static readonly string TagName = "email-domain";

        public string Value
        {
            get { return Data == null ? null : Data.Value; }
            set { Data.Value = value == null ? "" : value; }
        }
        public EmailDomain(XElement data)
            : base(data)
        {

        }

        public EmailDomain()
            : base(new XElement(TagName))
        {

        }
    }
}

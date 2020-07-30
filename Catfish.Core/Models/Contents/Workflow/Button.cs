using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class Button : XmlModel
    {
        public string Text { get; set; }
        public string Return { get; set; }
        public Button(XElement data)
            : base(data)
        {

        }
    }
}

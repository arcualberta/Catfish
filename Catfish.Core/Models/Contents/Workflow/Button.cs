using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class Button : XmlModel
    {
        public string Text
        {
            get => GetAttribute("text", null);
            set => SetAttribute("text", value);
        }

        public string Return
        {
            get => GetAttribute("return", null);
            set => SetAttribute("return", value);
        }
        
        public Button(XElement data)
            : base(data)
        {

        }
    }
}

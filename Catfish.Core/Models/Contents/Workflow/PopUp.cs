using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class PopUp : XmlModel
    {
        public string Title
        {
            get => GetAttribute("title", null as string);
            set => SetAttribute("title", value);
        }

        public string Message
        {
            get => GetAttribute("message", null as string);
            set => SetAttribute("message", value);
        }
        
        public PopUp(XElement data)
            : base(data)
        {

        }
    }
}

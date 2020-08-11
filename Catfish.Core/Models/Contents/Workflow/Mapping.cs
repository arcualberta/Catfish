using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class Mapping : XmlModel
    {
        public string Current
        {
            get => GetAttribute("current", null as string);
            set => SetAttribute("current", value);
        }

        public string Next
        {
            get => GetAttribute("next", null as string);
            set => SetAttribute("next", value);
        }

        public string ButtonLabel
        {
            get => GetAttribute("button-label", null as string);
            set => SetAttribute("button-label", value);
        }

        public Mapping(XElement data)
            : base(data)
        {

        }
        
    }
}

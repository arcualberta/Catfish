using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Workflow
{
    public class Map : XmlModel
    {
        public string Current { get; set; }
        public string Next { get; set; }
        public string ButtonLabel { get; set; }

        public Map(XElement data)
            : base(data)
        {

        }
    }
}

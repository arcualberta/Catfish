using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class TextField : AbstractField
    {
        public TextField() { }
        public TextField(XElement data) : base(data) { }

        public TextField(string name, string desc, string lang = null) : base(name, desc, lang) { }

        public override void Initialize()
        {
            
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class TextArea : TextField
    {
        public TextArea() : base() { }
        public TextArea(XElement data) : base(data) { }
        public TextArea(string name, string desc, string lang = null) : base(name, desc, lang) { }
    }
}

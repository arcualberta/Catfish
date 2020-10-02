using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class DecimalField : MonolingualTextField
    {
        public DecimalField() : base() { }
        public DecimalField(XElement data) : base(data) { }
        public DecimalField(string name, string desc, string lang = null) : base(name, desc, lang) { }
    }
}
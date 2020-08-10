using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class IntegerField : MonolingualTextField
    {
        public IntegerField() : base() { }
        public IntegerField(XElement data) : base(data) { }
        public IntegerField(string name, string desc, string lang = null) : base(name, desc, lang) { }
    }
}


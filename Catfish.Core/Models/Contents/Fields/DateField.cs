using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class DateField : MonolingualTextField
    {
        public DateField() : base() { }
        public DateField(XElement data) : base(data) { }
        public DateField(string name, string desc, string lang = null) : base(name, desc, lang) { }
    }
}

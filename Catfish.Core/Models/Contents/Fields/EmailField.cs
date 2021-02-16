using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class EmailField : MonolingualTextField
    {
        public EmailField() : base() { DisplayLabel = "Email"; }
        public EmailField(XElement data) : base(data) { DisplayLabel = "Email"; }
        public EmailField(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Email"; }
    }
}

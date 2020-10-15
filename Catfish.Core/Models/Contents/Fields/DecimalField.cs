using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class DecimalField : MonolingualTextField
    {
        public DecimalField() : base() { DisplayLabel = "Decimal Number"; }
        public DecimalField(XElement data) : base(data) { DisplayLabel = "Decimal Number"; }
        public DecimalField(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Decimal Number"; }
    }
}
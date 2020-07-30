using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class NumberField : TextField
    {
        public NumberField() : base() { }
        public NumberField(XElement data) : base(data) { }

    }
}

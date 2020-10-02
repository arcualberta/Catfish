using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class RadioField : OptionsField
    {
        public RadioField() : base() { }
        public RadioField(XElement data) : base(data) { }
    }
}

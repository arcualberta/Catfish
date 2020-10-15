using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class SelectField : OptionsField
    {
        public SelectField() : base() { DisplayLabel = "Dropdown"; }
        public SelectField(XElement data) : base(data) { DisplayLabel = "Dropdown"; }
    }
}

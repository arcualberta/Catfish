using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class CheckboxField : OptionsField
    {
        public CheckboxField() : base() { DisplayLabel = "Checkboxes"; }
        public CheckboxField(XElement data) : base(data) { DisplayLabel = "Checkboxes"; }
    }
}

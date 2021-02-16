using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class TableField : BaseField
    {
        public int MinRows
        {
            get => GetAttribute("min-rows", 0);
            set => SetAttribute("min-rows", value);
        }

        public int? MaxRows
        {
            get => GetAttribute("max-rows", null as int?);
            set => SetAttribute("max-rows", value);
        }

        public TableField() : base() { DisplayLabel = "Table"; }
        public TableField(XElement data) : base(data) { DisplayLabel = "Table"; }
        public TableField(string name, string desc, string lang = null) : base(name, desc, lang) { DisplayLabel = "Table"; }

        public override void UpdateValues(BaseField srcField)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class CheckboxField : OptionsField
    {
        public CheckboxField() : base() { DisplayLabel = "Checkboxes"; }
        public CheckboxField(XElement data) : base(data) { DisplayLabel = "Checkboxes"; }

        ////public override void UpdateValues(BaseField srcField)
        ////{
        ////    OptionsField src = srcField as OptionsField;
        ////    if (src == null)
        ////        throw new Exception("The source field is null or is not an OptionsField");

        ////    foreach (var dstOption in Options)
        ////    {
        ////        var isSrsOptionSelected = src.Options.Where(op => op.Id == dstOption.Id).Select(op => op.Selected).FirstOrDefault();
        ////        dstOption.Selected = isSrsOptionSelected;

        ////    }
        ////}
    }
}

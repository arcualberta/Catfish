using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class SelectField : OptionsField
    {
        public SelectField() : base() { DisplayLabel = "Dropdown"; }
        public SelectField(XElement data) : base(data) { DisplayLabel = "Dropdown"; }


        //public SelectField SetDefaultReferenceValue(SelectField refField, string delimiter = null, int interestedIndex = -1 /* index after parsing*/)
        //{
        //    var selectedOption = this.Options.Where(op => op.Selected).FirstOrDefault();
        //    string refValue = "";

        //    if ((selectedOption != null) && (delimiter != null))
        //        refValue = selectedOption.Id + ":[" + delimiter + "," + interestedIndex + "]";
        //    else
        //        refValue = refField.Id.ToString();

        //    this.SetAttribute("default-val-ref", refValue);

        //    return this;
        //}

    }
}

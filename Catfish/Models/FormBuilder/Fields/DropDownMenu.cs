using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.FormBuilder.Fields
{
    public class DropDownMenu : OptionField 
    {
        public DropDownMenu() : base("Select") { }
        public override BaseField CreateDataFieldFor(Core.Models.Contents.Form dataModel)
        {
            string lang = "en";
            
            BaseField dataField = dataModel.CreateField<SelectField>(Name, lang, Array.Empty<string>(), IsRequired).SetDescription(Description, lang);
            UpdateDataField(dataField);
            dataField.Id = Id;//set the dataField Id = with Id from vIew
            return dataField;
        }
    }
}
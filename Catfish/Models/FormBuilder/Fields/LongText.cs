using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.FormBuilder.Fields
{
    public class LongText : Field
    {
        public LongText() : base("Paragraph") { }
        public override BaseField CreateDataFieldFor(Core.Models.Contents.Form dataModel)
        {
            string lang = "en";
            BaseField dataField = dataModel.CreateField<TextArea>(Name, lang, IsRequired).SetDescription(Description, lang);
            dataField.Id = Id;//set the dataField Id = with Id from vIew
            return dataField;
        }
    }
}
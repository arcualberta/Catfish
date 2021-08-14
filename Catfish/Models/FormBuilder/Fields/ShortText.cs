using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Models.FormBuilder.Fields
{
    public class ShortText: Field
    {
        public ShortText() : base("Text") { }
        public ShortText(string templateButtonLabel) : base(templateButtonLabel) { }
        public override BaseField CreateDataFieldFor(Core.Models.Contents.Form dataModel)
            => CreateDataFieldFor<TextField>(dataModel);
    }
}
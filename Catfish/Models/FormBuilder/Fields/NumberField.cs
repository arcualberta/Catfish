using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.FormBuilder.Fields
{
    public class NumberField : ShortText
    {
        public int NumDecimalPlaces { get; set; }
        public NumberField() : base("Number") { }
        public override BaseField CreateDataFieldFor(Core.Models.Contents.Form dataModel)
            => NumDecimalPlaces > 0
                ? CreateDataFieldFor<DecimalField>(dataModel)
                : CreateDataFieldFor<IntegerField>(dataModel) as BaseField;
    }
}
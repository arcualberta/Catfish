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
            => CreateDataFieldFor<TextArea>(dataModel);
    }
}
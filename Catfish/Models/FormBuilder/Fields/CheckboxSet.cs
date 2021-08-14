using Catfish.Core.Models.Contents.Fields;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Models.FormBuilder.Fields
{
    public class CheckboxSet : OptionField
    {
        public CheckboxSet() : base("Check") { }
        /// <summary>
        /// convert FormBUilder's Checkboxfield to data field CheckboxField
        /// </summary>
        /// <param name="dataModel"></param>
        /// <returns></returns>
        public override BaseField CreateDataFieldFor(Core.Models.Contents.Form dataModel)
            => CreateDataFieldFor<CheckboxField>(dataModel);

    }
}
using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Helper
{
    public static class FieldRenderingHelper
    {
        public static Dictionary<string, object> CreateViewData(BaseField feild, string fieldRootId)
        {
            Dictionary<string, object> viewData = new Dictionary<string, object>();
            viewData.Add("data-model-id", feild.Id);
            viewData.Add("onchange", "updateFields();");
            if (feild.Readonly)
                viewData.Add("readonly", null as string);
            if (feild.RefId.HasValue)
                viewData.Add("data-ref-id", feild.RefId.Value);

            if (feild.Required)
            {
                viewData.Add("required", (string)null);
            }
            else
            {
                if (!string.IsNullOrEmpty(feild.VisibilityCondition.Value))
                    viewData.Add("data-visible-if", feild.VisibilityCondition.Value);

                if (!string.IsNullOrEmpty(feild.RequiredCondition.Value))
                    viewData.Add("data-required-if", feild.RequiredCondition.Value);

                if (!string.IsNullOrEmpty(fieldRootId))
                    viewData.Add("data-field-id", fieldRootId);
            }

            //Computed properties
            if (!string.IsNullOrEmpty(feild.ValueExpression.Value))
                viewData.Add("data-value-expression", feild.ValueExpression.Value);

            return viewData;
        }
    }
}

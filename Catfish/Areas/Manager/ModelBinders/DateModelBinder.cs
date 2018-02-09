using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Catfish.Core.ModelBinders
{
    public class DateModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (value.AttemptedValue.StartsWith("/Date("))
            {
                try
                {
                    DateTime date = new DateTime(1970, 01, 01, 0, 0, 0, DateTimeKind.Utc).ToUniversalTime();
                    string attemptedValue = value.AttemptedValue.Replace("/Date(", "").Replace(")/", "");
                    double milliSecondsOffset = Convert.ToDouble(attemptedValue);
                    DateTime result = date.AddMilliseconds(milliSecondsOffset);
                    result = result.ToUniversalTime();
                    return result;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}

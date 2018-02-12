using PerpetuumSoft.Knockout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Helpers
{
    public static class KnockoutMVCHelper
    {
        public static KnockoutTagBuilder<TModel> EditorFor<TModel>(this KnockoutContext<TModel> context, Expression<Func<TModel, object>> property, object additionalViewData = null, object htmlAttributes = null)
        {
            KnockoutTagBuilder<TModel> result = new KnockoutTagBuilder<TModel>(context, "div", null, null);
            if(property != null)
            {
                result.Value(property);
                var memberExpression = property.Body as MemberExpression;
                
            }

            return result;
        }
    }
}
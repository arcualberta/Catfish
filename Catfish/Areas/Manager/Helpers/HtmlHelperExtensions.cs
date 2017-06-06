using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Catfish.Core.Models.Metadata;
using System.Web.Mvc.Html;
using Catfish.Core.Models.Attributes;

namespace Catfish.Areas.Manager.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString EditorForList<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, ICollection<TValue>>> propertyExpression) where TModel : class where TValue : SimpleField
        {
            // Based on the tutorial from http://www.mattlunn.me.uk/blog/2014/08/how-to-dynamically-via-ajax-add-new-items-to-a-bound-list-model-in-asp-mvc-net/
            var items = propertyExpression.Compile()(html.ViewData.Model);
            StringBuilder htmlBuilder = new StringBuilder();
            string htmlFieldName = ExpressionHelper.GetExpressionText(propertyExpression);
            string htmlFieldNameWithPrefix = html.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);

            foreach(var item in items)
            {
                string guid = Guid.NewGuid().ToString().Replace('-', '_');

                var dummy = new { Item = item };
                var memberExp = Expression.MakeMemberAccess(Expression.Constant(dummy), dummy.GetType().GetProperty("Item"));
                var singleItemExp = Expression.Lambda<Func<TModel, TValue>>(memberExp, propertyExpression.Parameters);

                htmlBuilder.AppendFormat(@"<input type=""hidden"" name=""{0}.Index"" value=""{1}"" />", htmlFieldNameWithPrefix, guid);
                htmlBuilder.Append(html.EditorFor(singleItemExp, null, String.Format("{0}[{1}]", htmlFieldName, guid)));
                //CreateEntry<TModel, TValue>(item, htmlBuilder, propertyExpression, htmlFieldNameWithPrefix, htmlFieldName, html);
            }

            return new MvcHtmlString(htmlBuilder.ToString());
        }

        ////private static void CreateEntry<TModel, TValue>(TValue value, StringBuilder builder, Expression<Func<TModel, ICollection<TValue>>> propertyExpression, string htmlFieldNameWithPrefix, string htmlFieldName, HtmlHelper<TModel> html) where TModel : class where TValue : SimpleField
        ////{
        ////    string guid = Guid.NewGuid().ToString().Replace('-', '_');

        ////    var dummy = new { Item = value };
        ////    var memberExp = Expression.MakeMemberAccess(Expression.Constant(dummy), dummy.GetType().GetProperty("Item"));
        ////    var singleItemExp = Expression.Lambda<Func<TModel, TValue>>(memberExp, propertyExpression.Parameters);

        ////    builder.AppendFormat(@"<input type=""hidden"" name=""{0}.Index"" value=""{1}"" />", htmlFieldNameWithPrefix, guid);
        ////    builder.Append(html.EditorFor(singleItemExp, null, String.Format("{0}[{1}]", htmlFieldName, guid)));
        ////}

        public static MvcHtmlString TypeLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> propertyExpression)// where TModel : class where TValue : SimpleField
        {
            ModelMetadata metaData = ModelMetadata.FromLambdaExpression(propertyExpression, html.ViewData);

            string type_label = metaData.AdditionalValues.ContainsKey("TypeLabel") ? metaData.AdditionalValues["TypeLabel"] as string : typeof(TValue).ToString();

            return new MvcHtmlString(type_label);
        }
    }
}
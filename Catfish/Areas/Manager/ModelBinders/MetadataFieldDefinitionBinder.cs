
using Catfish.Core.Models.Metadata;
using System;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.ModelBinders
{
    public class MetadataFieldDefinitionBinder: DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            try
            {
                var key = bindingContext.ModelName + ".ModelType";
                var typeStr = bindingContext.ValueProvider.GetValue(key).AttemptedValue;
                var type = typeof(SimpleField).Assembly.GetType(typeStr);
                object obj = Activator.CreateInstance(type);
                return obj;
            }
            catch (Exception)
            {
                return base.CreateModel(controllerContext, bindingContext, modelType);
            }

        }

    }
}
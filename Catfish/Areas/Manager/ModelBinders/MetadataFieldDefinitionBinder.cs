
using Catfish.Core.Models.Metadata;
using System;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.ModelBinders
{
    public class MetadataFieldDefinitionBinder: DefaultModelBinder
    {
        ////protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        ////{
        ////    try
        ////    {
        ////        var key = bindingContext.ModelName + ".ModelType";
        ////        var typeStr = bindingContext.ValueProvider.GetValue(key).AttemptedValue;
        ////        var type = Type.GetType(typeStr);
        ////        object obj = Activator.CreateInstance(type);
        ////        return obj;
        ////    }
        ////    catch (Exception)
        ////    {
        ////        return base.CreateModel(controllerContext, bindingContext, modelType);
        ////    }
        ////}

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                var key = bindingContext.ModelName + ".ModelType";
                var typeStr = bindingContext.ValueProvider.GetValue(key).AttemptedValue;
                var type = Type.GetType(typeStr);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, type);
            }
            catch (Exception)
            {

            }
            return base.BindModel(controllerContext, bindingContext);
        }


    }
}
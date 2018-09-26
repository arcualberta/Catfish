using Catfish.Core.Models.Forms;
using System;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.ModelBinders
{
    public class XmlModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                var key = bindingContext.ModelName + ".ModelType";
                var typeObj = bindingContext.ValueProvider.GetValue(key);
                if(typeObj == null)
                    typeObj = bindingContext.ValueProvider.GetValue("ModelType");
                var typeStr = typeObj.AttemptedValue;
                var type = Type.GetType(typeStr);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, type);
            }
            catch (Exception ex)
            {

            }
            return base.BindModel(controllerContext, bindingContext);
        }

        protected override System.ComponentModel.ICustomTypeDescriptor GetTypeDescriptor(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                var key = bindingContext.ModelName + ".ModelType";
                var typeObj = bindingContext.ValueProvider.GetValue(key);
                if (typeObj == null)
                    typeObj = bindingContext.ValueProvider.GetValue("ModelType");
                var typeStr = typeObj.AttemptedValue;
                var type = Type.GetType(typeStr);
                return new System.ComponentModel.DataAnnotations.AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
            }
            catch (Exception ex)
            {

            }

            return base.GetTypeDescriptor(controllerContext, bindingContext);
        }
    }
}
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.ModelBinders
{
    public class FormFieldModelBinder : IModelBinder
    {
        private Dictionary<Type, (ModelMetadata, IModelBinder)> binders;

        public FormFieldModelBinder(Dictionary<Type, (ModelMetadata, IModelBinder)> binders)
        {
            this.binders = binders;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {

                if (bindingContext == null)
                    throw new ArgumentNullException(nameof(bindingContext));

                var modelKindName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, nameof(BaseField.ModelType));
                var modelTypeValue = bindingContext.ValueProvider.GetValue(modelKindName).FirstValue;

                if (string.IsNullOrEmpty(modelTypeValue))
                    return; // throw new Exception("No value was found for " + modelKindName);

                var modelType = Type.GetType(modelTypeValue);

                IModelBinder modelBinder;
                ModelMetadata modelMetadata;
                if (typeof(CheckboxField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(CheckboxField)];
                }
                else if (typeof(DateField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(DateField)];
                }
                else if (typeof(DecimalField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(DecimalField)];
                }
                else if (typeof(InfoSection).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(InfoSection)];
                }
                else if (typeof(IntegerField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(IntegerField)];
                }
                else if (typeof(MonolingualTextField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(MonolingualTextField)];
                }
                else if (typeof(RadioField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(RadioField)];
                }
                else if (typeof(SelectField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(SelectField)];
                }
                else if (typeof(TextArea).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(TextArea)];
                }
                else if (typeof(TextField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(TextField)];
                }
                else if (typeof(InfoSection).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(InfoSection)];
                }
                else if (typeof(CompositeField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(CompositeField)];
                }
                else if (typeof(AttachmentField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(AttachmentField)];
                }
                else if (typeof(AudioRecorderField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(AudioRecorderField)];
                }
                else if (typeof(EmailField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(EmailField)];
                }
                else if (typeof(TableField).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(TableField)];
                }
                else if (typeof(FieldContainerReference).IsAssignableFrom(modelType))
                {
                    (modelMetadata, modelBinder) = binders[typeof(FieldContainerReference)];
                }
                else
                {
                    (modelMetadata, modelBinder) = binders[typeof(BaseField)];
                }

                var newBindingContext = DefaultModelBindingContext.CreateBindingContext(
                    bindingContext.ActionContext,
                    bindingContext.ValueProvider,
                    modelMetadata,
                    bindingInfo: null,
                    bindingContext.ModelName);

                await modelBinder.BindModelAsync(newBindingContext).ConfigureAwait(false);
                bindingContext.Result = newBindingContext.Result;

                if (newBindingContext.Result.IsModelSet)
                {
                    // Setting the ValidationState ensures properties on derived types are correctly 
                    bindingContext.ValidationState[newBindingContext.Result] = new ValidationStateEntry
                    {
                        Metadata = modelMetadata,
                    };
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}

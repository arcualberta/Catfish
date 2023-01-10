using Catfish.Core.Models.Contents.Fields;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.ModelBinders
{
    public class FormFieldModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType != typeof(BaseField))
            {
                return null;
            }

            var subclasses = new[] { 
                typeof(CheckboxField), 
                typeof(DateField), 
                typeof(DecimalField), 
                typeof(InfoSection), 
                typeof(IntegerField), 
                typeof(MonolingualTextField), 
                typeof(OptionsField), 
                typeof(RadioField), 
                typeof(SelectField), 
                typeof(TextArea), 
                typeof(TextField),
                typeof(CompositeField),
                typeof(AttachmentField),
                 typeof(AudioRecorderField),
                typeof(EmailField),
                typeof(TableField),
                typeof(FieldContainerReference)
            };

            var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();
            foreach (var type in subclasses)
            {
                var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
                binders[type] = (modelMetadata, context.CreateBinder(modelMetadata));
            }

            return new FormFieldModelBinder(binders);
        }
    }
}

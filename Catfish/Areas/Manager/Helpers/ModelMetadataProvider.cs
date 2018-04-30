using Catfish.Core.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Catfish.Areas.Manager.Helpers
{
    public class ModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            CFTypeLabelAttribute attr = attributes.OfType<CFTypeLabelAttribute>().FirstOrDefault();
            if (attr != null)
            {
                metadata.AdditionalValues.Add("TypeLabel", attr.Name);
            }

            return metadata;
        }
    }
}
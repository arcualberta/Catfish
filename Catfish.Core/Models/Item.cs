using Catfish.Core.Models.Contents.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Catfish.Core.Models.Contents;
using Catfish.Core.Models.Contents.Fields;

namespace Catfish.Core.Models
{
    public class Item : Entity
    {
        /// <summary>
        /// If this container contains fieds that are of type FieldContainerReference, then
        /// see those referenced fields are available in the input arcgument "item" and updates
        /// them.
        /// </summary>
        /// <param name="item">The Item model which holds the referenced fielf contains either in its metadata sets or in its data contaner.</param>
        public void UpdateReferencedFieldContainers(FieldContainerBase src)
        {
            foreach (var field in src.Fields.Where(f => f is FieldContainerReference)
                .Select(f => f as FieldContainerReference))
            {
                FieldContainer dst = field.RefType == FieldContainerReference.eRefType.metadata
                     ? MetadataSets.FirstOrDefault(fc => fc.TemplateId == field.RefId)
                     : DataContainer.FirstOrDefault(fc => fc.Id == field.RefId) as FieldContainer;

                dst?.UpdateFieldValues(field.ChildForm);
            }
        }
    }
}

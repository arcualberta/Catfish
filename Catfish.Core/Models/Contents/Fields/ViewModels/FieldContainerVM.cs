using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Catfish.Core.Models.Contents.Fields.ViewModels
{
    //these are the MetadataSets
    public class FieldContainerVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<FieldVM> Fields { get; set; }

        public FieldContainerVM(MetadataSet src)
        {
            if (src != null)
                UpdateFieldContainerVM(src);
        }

        public void UpdateFieldContainerVM(MetadataSet metadataSet)
        {
            Id = metadataSet.Id;
            Name = metadataSet.Name.ConcatenatedContent;
            Fields = new List<FieldVM>();

            foreach (BaseField f in metadataSet.Fields)
            {
                Fields.Add(new FieldVM(f));
            }
        }

        /// <summary>
        /// This method iterates through each field inside "this" FieldContainerVM and
        /// update values of corresponding fields in the targetFiledContainer data model.
        /// </summary>
        /// <param name="targetFieldContainer"></param>
        public void UpdateFieldValues(FieldContainer targetFieldContainer)
        {
            foreach(var fieldVM in Fields)
            {
                //Find the corresponding field in the targetFiledContainer
                var targetField = targetFieldContainer.Fields.Where(f => f.Id == fieldVM.Id).FirstOrDefault();

                //This function does not have enough information to create new fields, so if we don't find
                //a matching field in the target, we can only throw an exception
                if (targetField == null)
                    throw new Exception(string.Format("The field specified by ID {0} not found in the target.", fieldVM.Id));

                //At this point, we found a matching field in the destinaiton.
                //Lets delegate the responsibility of updating the data model to the view model
                fieldVM.UpdateDataModel(targetField);
            }
        }
    }
}

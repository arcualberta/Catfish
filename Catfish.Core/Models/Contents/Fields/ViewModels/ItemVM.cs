using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Catfish.Core.Models.Contents.Fields.ViewModels
{
    public class ItemVM
    {
        public Guid Id { get; set; }
        public DisplayTextVM Name { get; set; } //this is correct... right? Not an array?
        public DisplayTextVM Description { get; set; } //^^
        public List<FieldContainerVM> MetadataSets { get; set; }

        public ItemVM() { MetadataSets = new List<FieldContainerVM>(); }

        public ItemVM(Item item)
        {
            Init(item);
        }


        public void Init(Item item)
        {
            Id = item.Id;
            Name = new DisplayTextVM(item.Name);
            Description = new DisplayTextVM(item.Description);

            MetadataSets = new List<FieldContainerVM>();

            //Builiding the metadata set of the view model by taking info from the data model
            foreach (MetadataSet metadataSet in item.MetadataSets)
            {
                MetadataSets.Add(new FieldContainerVM(metadataSet));
            }

        }

        public void UpdateDataModel(Item dataModel)
        {
            //Taking info in the metadata sets of the view model and updating the data model
            foreach (FieldContainerVM metadataSetVM in MetadataSets)
            {
                var targetDataModelMetadataSet = dataModel.MetadataSets
                    .Where(ms => ms.Id == metadataSetVM.Id)
                    .FirstOrDefault();

                //We are not going to create new metadata sets in this editor interface. Therefore, if the
                //requested metadata set is not found due to whatever reason, we through an error.
                if (targetDataModelMetadataSet == null)
                    throw new Exception(string.Format("Metadata set identified by {0} not found in the data model.", metadataSetVM.Id));

                //Delegating the task of updating the metadata set in the data model 
                //to the view model. In this case, we simply need to update field values of the metadata set
                metadataSetVM.UpdateFieldValues(targetDataModelMetadataSet);
            }

        }
    }
}

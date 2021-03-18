using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Catfish.Core.Models.Contents.Fields.ViewModels
{
    public class ItemVM
    {
        public Guid Id { get; set; }
        public List<DisplayTextVM> Name { get; set; }
        public List<DisplayTextVM> Description { get; set; }
        public List<FieldContainerVM> MetadataSets { get; set; }

        public ItemVM() { MetadataSets = new List<FieldContainerVM>(); }

        public ItemVM(Item item)
        {
            Init(item);
        }


        public void Init(Item item)
        {
            Id = item.Id;

            Name = new List<DisplayTextVM>();
            Description = new List<DisplayTextVM>();
            foreach (Text txt in item.Name.Values)
            {
                Name.Add(new DisplayTextVM(txt));
            }

            foreach(Text txt in item.Description.Values)
            {
                Description.Add(new DisplayTextVM(txt));
            }

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

            //Handle the Name and Description lists similarly
            //Currently, we are not allowing the user to add/delete values for these

            foreach(DisplayTextVM name in Name)
            {
                var targetDataModelName = dataModel.Name.Values
                    .Where(ms => ms.Id == name.Id)
                    .FirstOrDefault();

                if (targetDataModelName == null)
                    throw new Exception(string.Format("Name set identified by {0} not found in the data model.", name.Id));

                name.UpdateFieldValues(targetDataModelName);
            }

            foreach (DisplayTextVM description in Description)
            {
                var targetDataModelDescription = dataModel.Description.Values
                    .Where(ms => ms.Id == description.Id)
                    .FirstOrDefault();

                if (targetDataModelDescription == null)
                    throw new Exception(string.Format("Description set identified by {0} not found in the data model.", description.Id));

                description.UpdateFieldValues(targetDataModelDescription);
            }
        }
    }
}

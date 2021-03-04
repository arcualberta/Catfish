using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Fields.ViewModels
{
    class ItemVM
    {
        public Guid Id { get; set; }
        public List<DisplayTextVM> Name { get; set; }
        public List<DisplayTextVM> Description { get; set; }
        public List<FieldContainerVM> MetadataSets { get; set; }

        public void UpdateItemVM(Item item)
        {
            Name = new List<DisplayTextVM>();
            Description = new List<DisplayTextVM>();
            MetadataSets = new List<FieldContainerVM>();

            //has metadata items inside
            foreach (MetadataSet metadataSet in item.MetadataSets)
            {
                MetadataSets.Add(new FieldContainerVM(metadataSet) );
            }

            //it seems there is more than one Text value per MultilingualName though? Or am I confused
            foreach (MultilingualName multiName in item.Name)
            {
                Name.Add(new DisplayTextVM(multiName) );
            }

            foreach (MultilingualName multiDesc in item.Description)
            {
                Description.Add(new DisplayTextVM(multiDesc) );
            }
        }


    }
}

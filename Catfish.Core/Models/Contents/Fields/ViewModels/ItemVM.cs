using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Fields.ViewModels
{
    public class ItemVM
    {
        public Guid Id { get; set; }
        public DisplayTextVM Name { get; set; } //this is correct... right? Not an array?
        public DisplayTextVM Description { get; set; } //^^
        public List<FieldContainerVM> MetadataSets { get; set; }

        public ItemVM() { }

        public ItemVM(Item item)
        {
            Init(item);
        }


        public void Init(Item item)
        {
            MetadataSets = new List<FieldContainerVM>();

            //has metadata items inside
            foreach (MetadataSet metadataSet in item.MetadataSets)
            {
                MetadataSets.Add(new FieldContainerVM(metadataSet));
            }

            Name = new DisplayTextVM(item.Name);
            Description = new DisplayTextVM(item.Description);
            Id = Guid.NewGuid();
        }
    }
}

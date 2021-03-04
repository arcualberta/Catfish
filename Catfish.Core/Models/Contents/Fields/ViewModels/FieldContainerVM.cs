using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Fields.ViewModels
{
    //these are the MetadataSets
    class FieldContainerVM
    {
        public Guid Id { get; set; }
        public List<Field> Fields { get; set; }

        public FieldContainerVM(MetadataSet src)
        {
            UpdateFieldContainerVM(src);
        }

        public void UpdateFieldContainerVM(MetadataSet metadataSet)
        {
            Fields = new List<Field>();

            //metadataSets.Fields is a FieldList/BaseField type, do I need to extend Field? Something else?
            //added another constructor to Field, not sure if that is the right thing to do
            foreach(BaseField f in metadataSet.Fields)
            {
                Fields.Add(new Field(f));
            }
        }
    }
}

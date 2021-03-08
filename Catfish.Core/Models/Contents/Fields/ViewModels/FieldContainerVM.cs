using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.Contents.Fields.ViewModels
{
    //these are the MetadataSets
    public class FieldContainerVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Field> Fields { get; set; }

        public FieldContainerVM(MetadataSet src)
        {
            UpdateFieldContainerVM(src);
        }

        public void UpdateFieldContainerVM(MetadataSet metadataSet)
        {
            Name = metadataSet.Name.ConcatenatedContent;
            Fields = new List<Field>();

            foreach(BaseField f in metadataSet.Fields)
            {
                Fields.Add(new Field(f));
            }
        }
    }
}

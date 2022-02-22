using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.ViewModels
{
    class TemplateCollectionVM
    {
        public Guid CollectionId { get; set; }
        public Guid? TemplateGroupId { get; set; }
        public string CollectionName { get; set; }
        public bool Assigned { get; set; }
    }
}

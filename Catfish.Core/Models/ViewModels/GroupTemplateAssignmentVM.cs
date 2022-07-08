using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models.ViewModels
{
    public class GroupTemplateAssignmentVM
    {
        public Guid TemplateId { get; set; }
        public Guid? TemplateGroupId { get; set; }
        public string TemplateName { get; set; }
        public bool Assigned { get; set; }
        public bool HasCollections { get; set; }
    }
}

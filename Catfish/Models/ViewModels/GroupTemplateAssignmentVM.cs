using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Models.ViewModels
{
    public class GroupTemplateAssignmentVM
    {
        public Guid TemplateId { get; set; }
        public Guid? TemplateGroupId { get; set; }
        public string TemplateName { get; set; }
        public bool Assigned { get; set; }
    }
}

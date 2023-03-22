using CatfishExtensions.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO.Entity
{
    public class EntityTemplateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public eState State { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

      

       // public ICollection<FormTemplate> Forms { get; set; } = new List<FormTemplate>();

       // public ICollection<WorkflowDbRecord> Workflows { get; set; } = new List<WorkflowDbRecord>();
    }
}

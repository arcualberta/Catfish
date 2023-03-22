using CatfishExtensions.Constants;
using CatfishExtensions.DTO.Forms;
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


        public EntityTemplateSettings? EntityTemplateSettings { get; set; }
        public ICollection<FormTemplateDto> Forms { get; set; } = new List<FormTemplateDto>();

       // public ICollection<WorkflowDbRecord> Workflows { get; set; } = new List<WorkflowDbRecord>();
    }
}

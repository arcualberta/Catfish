using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO.Entity
{
    public class RelationshipDto
    {
        public Guid SubjectEntityId { get; set; }
        public  EntityDataDto SubjectEntity { get; set; }

        public Guid ObjectEntityId { get; set; }
        public  EntityDataDto ObjectEntity { get; set; }

        public string Name { get; set; }
        public int Order { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO.Entity
{
    public class Relationship
    {
        public Guid SubjectEntityId { get; set; }
        public  EntityData SubjectEntity { get; set; }

        public Guid ObjectEntityId { get; set; }
        public  EntityData ObjectEntity { get; set; }

        public string Name { get; set; }
        public int Order { get; set; }
    }
}

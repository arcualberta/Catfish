using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models
{
    public class Collection : Entity
    {
        public GroupTemplate GroupTemplate { get; set; }
        public Guid? GroupTemplateId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catfish.Core.Models
{
    [Table("Catfish_SystemStatuses")]
    public class SystemStatus
    {
        public Guid Id { get; set; }
        public string SystemName { get; set; }
        public string Status { get; set; }

    }
}

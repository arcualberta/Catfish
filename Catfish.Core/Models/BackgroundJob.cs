using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catfish.Core.Models
{
    [Table("Catfish_BackgroundJobs")]
    public class BackgroundJob
    {
        public Guid Id { get; set; }
        public long HangfireJobId { get; set; }
        public Guid? EntityId { get; set; }
        public Guid? SourceTaskId { get; set; }
        public string Task { get; set; }
    }
}

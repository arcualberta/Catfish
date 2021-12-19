using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catfish.Core.Models
{
    [Table("Catfish_IndexingHistory")]
    public class IndexingHistory
    {
        public Guid Id { get; set; }
        public Guid ObjectId { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastIndexedAt { get; set; }

        public IndexingHistory()
        {
            Created = DateTime.Now;
        }
    }
}

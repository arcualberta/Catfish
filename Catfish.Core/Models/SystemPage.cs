using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catfish.Core.Models
{
    [Table("Catfish_SystemPages")]
    public class SystemPage
    {
        public int Id { get; set; }
        public Guid SiteId { get; set; }
        public Guid PageId { get; set; }
        public string PageKey { get; set; }
    }
}

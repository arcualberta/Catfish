using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catfish.Core.Models
{
    [Table("Catfish_Backup")]
    public class Backup
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public string SourceType { get; set; }
        public string SourceData { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public Backup() { }

        public Backup(Guid srcId, string srcType, string srcData, Guid userId, string username)
        {
            SourceId = srcId;
            SourceType = srcType;
            SourceData = srcData;
            UserId = userId;
            Username = username;
            Timestamp = DateTime.Now;
        }
    }
}

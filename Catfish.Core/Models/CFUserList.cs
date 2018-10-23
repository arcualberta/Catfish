using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Core.Models
{
    public class CFUserList
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<CFUserListEntry> CFUserListEntries { get; set; } //user ids

        public CFUserList()
        {
            CFUserListEntries = new List<CFUserListEntry>();
        }
        
    }
    public class CFUserListEntry //EntityGroupUser
    {
        [Key]
        public Guid CFUserListId { get; set; }
        [Key]
        public Guid UserId { get; set; }
        
    }
}
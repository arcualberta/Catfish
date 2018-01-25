using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Core.Models
{
    public class EntityGroup
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<EntityGroupUser> EntityGroupUsers { get; set; } //user ids

        public EntityGroup()
        {
            EntityGroupUsers = new List<EntityGroupUser>();
        }
        
    }
    public class EntityGroupUser
    {
        [Key]
        public Guid EntityGroupId { get; set; }
        [Key]
        public Guid UserId { get; set; }
        
    }
}
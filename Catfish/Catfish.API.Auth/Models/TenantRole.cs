﻿

namespace Catfish.API.Auth.Models
{
    public class TenantRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Tenant Tenant { get; set; }
        public Guid TenantId { get; set; }
        public ICollection<TenantUser> Users { get; set; }
    }
}

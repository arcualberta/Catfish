using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catfish.Core.Models
{
    [Table("Catfish_GroupTemplates")]
    public class GroupTemplate
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
        public Guid EntityTemplateId { get; set; }
        public EntityTemplate EntityTemplate { get; set; }

        public ICollection<Collection> Collections { get; set; } = new List<Collection>();
    }
    public class GroupTemplateComparer : IEqualityComparer<GroupTemplate>
    {
        public bool Equals(GroupTemplate x, GroupTemplate y)
        {
            if (x.EntityTemplateId == y.EntityTemplateId && x.GroupId == y.GroupId)
                return true;

            return false;
        }

        public int GetHashCode(GroupTemplate obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}

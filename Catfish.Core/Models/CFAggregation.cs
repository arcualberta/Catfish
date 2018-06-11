using Catfish.Core.Models.Access;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Catfish.Core.Models
{
    [Serializable]
    public abstract class CFAggregation : CFEntity
    {
        [IgnoreDataMember]
        public virtual ICollection<CFAggregation> ParentMembers { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<CFAggregation> ChildMembers { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<CFItem> ChildRelations { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public bool HasAssociations { get { return ParentMembers.Count > 0 || ChildMembers.Count > 0 || ChildRelations.Count > 0; } }


        public CFAggregation()
        {
            ParentMembers = new List<CFAggregation>();
            ChildMembers = new List<CFAggregation>();
            ChildRelations = new List<CFItem>();
        }

        /// <summary>
        /// WARNING: Check for circular references first!
        /// </summary>
        /// <param name="child"></param>
        public void AppendChild(CFAggregation child)
        {
            this.ChildMembers.Add(child);
            child.ParentMembers.Add(this);
            Guid childGuid = new Guid(child.Guid);
            AccessMode parentsAccessModes = RecalculateInheritedPermissions(childGuid);


            child.SetAccess(parentsAccessModes);
        }

        /// <summary>
        /// WARNING: Check for circular references first!
        /// </summary>
        /// <param name="child"></param>
        public void AppendParent(CFAggregation parent)
        {
            parent.ChildMembers.Add(this);
            this.ParentMembers.Add(parent);
        }

        [IgnoreDataMember]
        public virtual IEnumerable<CFAggregation> ChildItems
        {
            get
            {
                return ChildMembers.Where(c => typeof(CFItem).IsAssignableFrom(c.GetType()));
            }
        }

        protected AccessMode RecalculateInheritedPermissions(Guid guid)
        {
            AccessMode accessMode = AccessMode.None;

            foreach (CFAggregation parent in ParentMembers)
            {
                CFAccessGroup accessGroup = parent.AccessGroups
                    .Where(x => x.Guid == guid.ToString()).FirstOrDefault();
                if (accessGroup != null)
                {
                    accessMode = accessMode | accessGroup.AccessDefinition.AccessModes;
                }
            }

            return accessMode;
        }

        public new void SetAccess(AccessMode accessMode, bool isInherited = false)
        {
            //XXX Check control permission

            Guid guid = new Guid(this.Guid);

            CFAccessGroup accessGroup = GetAccessGroup(guid);

            if (accessGroup == null)
            {
                base.SetAccess(accessMode, isInherited);
                return;
            } else if (accessGroup.IsInherited == false && isInherited == true)
            {
                return;
            }

            AccessGroups.Remove(accessGroup);
            CFAccessGroup newAccessGroup = new CFAccessGroup();
            newAccessGroup.IsInherited = isInherited;
            newAccessGroup.Guid = guid.ToString();
            newAccessGroup.AccessDefinition.AccessModes = accessMode;
            
            if (accessGroup.IsInherited == true && isInherited == true)
            {
                AccessMode inheritedAccessMode = RecalculateInheritedPermissions(guid);
                newAccessGroup.AccessDefinition.AccessModes |= inheritedAccessMode;
            }

            AccessGroups.Add(newAccessGroup);
            
            foreach (CFAggregation child in ChildMembers)
            {
                child.SetAccess(accessMode, true);
            }
        }

    }
}
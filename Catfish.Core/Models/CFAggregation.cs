using Catfish.Core.Models.Access;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace Catfish.Core.Models
{
    [Serializable]
    public class CFAggregation : CFEntity
    {

        public override string GetTagName() { return "aggregation"; }
        public override string Label =>"Aggregation";

        [IgnoreDataMember]
        [InverseProperty("Child")]
        public virtual List<CFAggregationHasMembers> ConnectionToParents { get; set; }

        [IgnoreDataMember]
        [InverseProperty("Parent")]
        public virtual List<CFAggregationHasMembers> ConnectionToChildren { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        protected object ChildConnectionLock = new object(); // Used to synchronize addition of child members;
        
        [IgnoreDataMember]
        public virtual List<CFItem> ManagedRelatedMembers { get; set; }

        //[JsonIgnore]
        [IgnoreDataMember]
        public virtual IReadOnlyCollection<CFAggregation> ParentMembers {
            get {
                return ConnectionToParents.Select(m => m.Parent).ToList().AsReadOnly();
            }
        }

        //[JsonIgnore]
        [IgnoreDataMember]
        public virtual IReadOnlyCollection<CFAggregation> ChildMembers {
            get
            {
                return ConnectionToChildren.OrderBy(m => m.Order).Select(m => m.Child).ToList().AsReadOnly();
            }
        }

        //[JsonIgnore]
        [IgnoreDataMember]
        public virtual IReadOnlyCollection<CFItem> RelatedMembers
        {
            get
            {
                return ManagedRelatedMembers.AsReadOnly();
            }
        }

        ////[JsonIgnore]
        //[IgnoreDataMember]
        //public virtual ICollection<CFItem> ChildRelations { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public bool HasAssociations { get { return ParentMembers.Count > 0 || ChildMembers.Count > 0 || RelatedMembers.Count > 0; } }


        public CFAggregation()
        {
            ConnectionToChildren = new List<CFAggregationHasMembers>();
            ConnectionToParents = new List<CFAggregationHasMembers>();
            ManagedRelatedMembers = new List<CFItem>();
        }

        protected void RefreshChildrenOrdering(int order, int increment)
        {
            lock (ChildConnectionLock)
            {
                foreach(CFAggregationHasMembers connection in ConnectionToChildren)
                {
                    if(connection.Order >= order)
                    {
                        connection.Order += increment;
                    }
                }
            }
        }

        /// <summary>
        /// WARNING: Check for circular references first!
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(CFAggregation child, int order = -1)
        {
            lock (ChildConnectionLock)
            {
                CFAggregationHasMembers member = new CFAggregationHasMembers();
                member.Parent = this;
                member.Child = child;

                if (order < 0 || order >= ConnectionToChildren.Count)
                {
                    member.Order = ConnectionToChildren.Count;
                    ConnectionToChildren.Add(member);
                }else
                {
                    member.Order = order;
                    //RefreshChildrenOrdering(order, 1); // This is currently breaking the system
                    ConnectionToChildren.Add(member);
                    
                }

                child.ConnectionToParents.Add(member);

                this.AccessGroups.ForEach(x => child.SetAccess(x.AccessGuid,
                    x.AccessDefinition.AccessModes,
                    true));
            }
        }

        public void RemoveChild(CFAggregationHasMembers connection)
        {
            lock (ChildConnectionLock)
            {
                CFAggregation child = connection.Child;

                ConnectionToChildren.Remove(connection);
                child.ConnectionToParents.Remove(connection);
                child.AccessGroups.RemoveAll(x => x.IsInherited == true);

                foreach (CFAggregationHasMembers parent in child.ConnectionToParents)
                {
                    parent.Parent.AccessGroups.ForEach(x => child.SetAccess(
                        x.AccessGuid,
                        x.AccessDefinition.AccessModes,
                        true));
                }

                //RefreshChildrenOrdering(connection.Order + 1, -1); // This is currently breaking the system
            }
        }
       
        public void RemoveChild(CFAggregation child)
        {
            lock (ChildConnectionLock)
            {
                CFAggregationHasMembers childConnection = ConnectionToChildren.Where(connection => connection.ChildId == child.Id).FirstOrDefault();

                if (childConnection != null)
                {
                    RemoveChild(childConnection);
                }
            }
        }

        public void AddRelated(CFItem related)
        {
            ManagedRelatedMembers.Add(related);
        }

        public void RemoveRelated(CFItem related)
        {
            ManagedRelatedMembers.Remove(related);
        }

        /// <summary>
        /// WARNING: Check for circular references first!
        /// </summary>
        /// <param name="child"></param>
        //public void AppendParent(CFAggregation parent)
        //{
        //    parent.ChildMembers.Add(this);
        //    this.ParentMembers.Add(parent);
        //}

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

        public new void SetAccess(Guid guid, AccessMode accessMode, bool isInherited = false)
        {
            //XXX Check control permission
            //User.Identity.IsAuthenticated
            CFAccessGroup accessGroup = GetAccessGroup(guid);

            if (accessGroup == null)
            {
                base.SetAccess(guid, accessMode, isInherited);
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
                child.SetAccess(guid, accessMode, true);
            }
        }

        override public Dictionary<string, object> ToSolrDictionary()
        {
            Dictionary<string, object> solrDictionary = base.ToSolrDictionary();
            //solrDictionary["modeltype_s"] = "testingcall";
            solrDictionary["parents_ss"] = ParentMembers.Select(x => x.Guid).ToList();
            solrDictionary["related_ss"] = RelatedMembers.Select(x => x.Guid).ToList();
            return solrDictionary;
        }

    }
}
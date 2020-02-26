using Catfish.Core.Models.Access;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
//using System.Web.Script.Serialization;

namespace Catfish.Core.Models
{
    [Table("Catfish_Aggregations")]
    [Serializable]
    public class Aggregation : Entity
    {

        public override string GetTagName() { return "aggregation"; }
        public override string Label =>"Aggregation";
        private List<Aggregation> resetSetAccess = new List<Aggregation>();

        [IgnoreDataMember]
        [InverseProperty("Child")]
        public virtual List<AggregationHasMembers> ConnectionToParents { get; set; }

        [IgnoreDataMember]
        [InverseProperty("Parent")]
        public virtual List<AggregationHasMembers> ConnectionToChildren { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        protected object ChildConnectionLock = new object(); // Used to synchronize addition of child members;

        [IgnoreDataMember]
        public virtual List<Item> ManagedRelatedMembers { get; set; }

        //[JsonIgnore]
        [IgnoreDataMember]
        public virtual IReadOnlyCollection<Aggregation> ParentMembers {
            get {
                return ConnectionToParents.Select(m => m.Parent).ToList().AsReadOnly();
            }
        }

        //[JsonIgnore]
        [IgnoreDataMember]
        public virtual IReadOnlyCollection<Aggregation> ChildMembers {
            get
            {
                return ConnectionToChildren.OrderBy(m => m.Order).Select(m => m.Child).ToList().AsReadOnly();
            }
        }

        //[JsonIgnore]
        [IgnoreDataMember]
        public virtual IReadOnlyCollection<Item> RelatedMembers
        {
            get
            {
                return ManagedRelatedMembers.AsReadOnly();
            }
        }

        //[JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Relation> Relations { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public bool HasAssociations { get { return ParentMembers.Count > 0 || ChildMembers.Count > 0 || RelatedMembers.Count > 0; } }


        public Aggregation()
        {
            ConnectionToChildren = new List<AggregationHasMembers>();
            ConnectionToParents = new List<AggregationHasMembers>();
            Relations = new List<Relation>();
        }

        protected void RefreshChildrenOrdering(int order, int increment)
        {
            lock (ChildConnectionLock)
            {
                foreach(AggregationHasMembers connection in ConnectionToChildren)
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
        public void AddChild(Aggregation child, int order = -1)
        {
            lock (ChildConnectionLock)
            {
                AggregationHasMembers member = new AggregationHasMembers();
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

                child.ResetPermissions();
            }
        }

        public void RemoveChild(AggregationHasMembers connection)
        {
            lock (ChildConnectionLock)
            {
                Aggregation child = connection.Child;

                ConnectionToChildren.Remove(connection);
                child.ConnectionToParents.Remove(connection);
                child.ResetPermissions();

                //RefreshChildrenOrdering(connection.Order + 1, -1); // This is currently breaking the system
            }
        }
       
        public void RemoveChild(Aggregation child)
        {
            lock (ChildConnectionLock)
            {
                AggregationHasMembers childConnection = ConnectionToChildren.Where(connection => connection.ChildId == child.Id).FirstOrDefault();

                if (childConnection != null)
                {
                    RemoveChild(childConnection);
                }
            }
        }

        public void AddRelated(Item related)
        {
            ManagedRelatedMembers.Add(related);
        }

        public void RemoveRelated(Item related)
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
        public virtual IEnumerable<Aggregation> ChildItems
        {
            get
            {
                return ChildMembers.Where(c => typeof(Item).IsAssignableFrom(c.GetType()));
            }
        }

        protected AccessMode RecalculateInheritedPermissions(Guid guid)
        {
            AccessMode accessMode = AccessMode.None;

            foreach (Aggregation parent in ParentMembers)
            {
                CFAccessGroup accessGroup = parent.AccessGroups
                    .Where(x => x.Guid == guid).FirstOrDefault();
                if (accessGroup != null)
                {
                    accessMode = accessMode | accessGroup.AccessDefinition.AccessModes;
                }
            }

            return accessMode;
        }

        //private void ResetPermissions(CFAggregation agregation)
        //{
        //    List<CFAccessGroup> accessGroups = agregation.AccessGroups.ToList();
        //    accessGroups.RemoveAll(x => x.IsInherited == true);
        //    agregation.AccessGroups = accessGroups;

        //    foreach (CFAggregation parent in agregation.ParentMembers)
        //    {
        //        if (parent != this && parent != agregation)
        //        {
        //            foreach (CFAccessGroup accessGroup in parent.AccessGroups)
        //            {
        //                agregation.SetAccess(accessGroup.AccessGuid,
        //                    accessGroup.AccessDefinition.AccessModes,
        //                    true);
        //            }
        //        }
        //    }

        //    foreach (CFAggregation child in agregation.ChildMembers)
        //    {
        //        if (child != this && child != agregation)
        //        {
        //            ResetPermissions(child);
        //        }                
        //    }
        //}

        private void ResetPermissions()
        {
            List<CFAccessGroup> accessGroups = AccessGroups.ToList();
            accessGroups.RemoveAll(x => x.IsInherited == true);
            AccessGroups = accessGroups;

            foreach (Aggregation parent in ParentMembers)
            {
                if (parent != this)
                {
                    foreach (CFAccessGroup accessGroup in parent.AccessGroups)
                    {
                        SetAccess(accessGroup.AccessGuid,
                            accessGroup.AccessDefinition.AccessModes,
                            true);
                    }
                }
            }

            foreach (Aggregation child in ChildMembers)
            {
                if (child != this)
                {
                    child.ResetPermissions();
                }
            }
        }
        
        /**
         * Recalculates the current aggrigation's inherited 
         **/
        public void RecalculateInheritedPermissions()
        {
            foreach (Aggregation parent in ParentMembers)
            {
                if (parent != this)
                {
                    foreach (CFAccessGroup accessGroup in parent.AccessGroups)
                    {
                        SetAccess(accessGroup.AccessGuid,
                            accessGroup.AccessDefinition.AccessModes,
                            true);
                    }
                }
            }
        }

        public new void SetAccess(Guid guid, AccessMode accessMode, bool isInherited = false)
        {
            //XXX Check control permission
            //User.Identity.IsAuthenticated
            CFAccessGroup accessGroup = GetAccessGroup(guid);

            if (accessGroup == null)
            {
                base.SetAccess(guid, accessMode, isInherited);
            }
            else if (accessGroup.IsInherited == false && isInherited == true)
            {
                return;
            }
            else
            {
                List<CFAccessGroup> accessGroups = AccessGroups.ToList() ;
                accessGroups.Remove(accessGroup);
                CFAccessGroup newAccessGroup = new CFAccessGroup();
                newAccessGroup.IsInherited = isInherited;
                newAccessGroup.Guid = guid;
                newAccessGroup.AccessDefinition.AccessModes = accessMode;

                if (accessGroup.IsInherited == true && isInherited == true)
                {
                    AccessMode inheritedAccessMode = RecalculateInheritedPermissions(guid);
                    newAccessGroup.AccessDefinition.AccessModes |= inheritedAccessMode;
                }

                accessGroups.Add(newAccessGroup);
                AccessGroups = accessGroups;
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

        public void VisitHierarchy(Action<Aggregation> visitor)
        {
            List<Aggregation> toVisit = new List<Aggregation>
            {
                this
            };

            for (int i = 0; i < toVisit.Count; ++i)
            {
                Aggregation currentAggregation = toVisit[i];
                visitor(currentAggregation);
                foreach (Aggregation child in currentAggregation.ChildMembers)
                {
                    //child.Guid
                    //if (!toVisit.Any(x => x.Guid == child.Guid))
                    if (!toVisit.Contains(child))
                    {
                        toVisit.Add(child);
                    }
                }
            }
        }

        public bool Equals(Aggregation other)
        {
            if (other == null)
            {
                return false;
            }

            return Guid == other.Guid &&
                Id == other.Id;
        }

    }
}
 
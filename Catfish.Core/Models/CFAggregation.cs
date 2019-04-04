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
        private List<CFAggregation> childrenSetAccess = new List<CFAggregation>();
        private List<CFAggregation> resetSetAccess = new List<CFAggregation>();

        [IgnoreDataMember]
        public virtual List<CFAggregation> ManagedParentMembers { get; set; }

        [IgnoreDataMember]
        public virtual List<CFAggregation> ManagedChildMembers { get; set; }

        [IgnoreDataMember]
        public virtual List<CFItem> ManagedRelatedMembers { get; set; }

        //[JsonIgnore]
        [IgnoreDataMember]
        public virtual IReadOnlyCollection<CFAggregation> ParentMembers {
            get {
                return ManagedParentMembers.AsReadOnly();
            }
        }

        //[JsonIgnore]
        [IgnoreDataMember]
        public virtual IReadOnlyCollection<CFAggregation> ChildMembers {
            get
            {
                return ManagedChildMembers.AsReadOnly();
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
            ManagedParentMembers = new List<CFAggregation>();
            ManagedChildMembers = new List<CFAggregation>();
            ManagedRelatedMembers = new List<CFItem>();
        }

        /// <summary>
        /// WARNING: Check for circular references first!
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(CFAggregation child)
        {
            ManagedChildMembers.Add(child);
            child.ManagedParentMembers.Add(this);
            ResetPermissions(child);
            //AccessGroups.Select(x => child.SetAccess(x.AccessGuid, 
            //    x.AccessDefinition.AccessModes, 
            //    true));

            //foreach (CFAccessGroup accessGroup in AccessGroups)
            //{
            //    child.SetAccess(accessGroup.AccessGuid,
            //        accessGroup.AccessDefinition.AccessModes,
            //        true);
            //}

        }

       
        public void VisitHierarchy(Action<CFAggregation> visitor)
        {
            List<CFAggregation> toVisit = new List<CFAggregation>
            {
                this
            };

            for(int i=0; i<toVisit.Count; ++i)
            {
                CFAggregation currentAggregation = toVisit[i];
                visitor(currentAggregation);
                foreach (CFAggregation child in currentAggregation.ChildMembers)
                {
                    //child.Guid
                    //if (!toVisit.Any(x => x.Guid == child.Guid))
                    if(!toVisit.Contains(child))
                    {
                        toVisit.Add(child);
                    }
                }
            }
        }

        public bool Equals(CFAggregation other)
        {
            if (other == null)
            {
                return false;
            }

            return Guid == other.Guid && 
                Id == other.Id;
        }

        private void ResetPermissions(CFAggregation agregation)
        {
            List<CFAccessGroup> accessGroups = agregation.AccessGroups.ToList();
            accessGroups.RemoveAll(x => x.IsInherited == true);
            agregation.AccessGroups = accessGroups;

            foreach (CFAggregation parent in agregation.ManagedParentMembers)
            {                
                foreach (CFAccessGroup accessGroup in parent.AccessGroups)
                {
                    agregation.SetAccess(accessGroup.AccessGuid,
                        accessGroup.AccessDefinition.AccessModes,
                        true);
                }
            }
            // XXX aqui es otra

            foreach (CFAggregation child in agregation.ChildMembers)
            {
                if (! resetSetAccess.Contains(child))
                {
                    resetSetAccess.Add(child);
                    ResetPermissions(child);
                }
                
            }
        }

        public void RemoveChild(CFAggregation child)
        {
            ManagedChildMembers.Remove(child);
            child.ManagedParentMembers.Remove(this);
            ResetPermissions(child);
            //List<CFAccessGroup> accessGroups = child.AccessGroups.ToList();
            //accessGroups.RemoveAll(x => x.IsInherited == true);
            //child.AccessGroups = accessGroups;

            //foreach (CFAggregation parent in child.ManagedParentMembers)
            //{
            //    //parent.AccessGroups.ForEach(x => child.SetAccess(
            //    //    x.AccessGuid, 
            //    //    x.AccessDefinition.AccessModes, 
            //    //    true));

            //    foreach(CFAccessGroup accessGroup in parent.AccessGroups)
            //    {
            //        child.SetAccess(accessGroup.AccessGuid,
            //            accessGroup.AccessDefinition.AccessModes,
            //            true);
            //    }

            //}            
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
                newAccessGroup.Guid = guid.ToString();
                newAccessGroup.AccessDefinition.AccessModes = accessMode;

                if (accessGroup.IsInherited == true && isInherited == true)
                {
                    AccessMode inheritedAccessMode = RecalculateInheritedPermissions(guid);
                    newAccessGroup.AccessDefinition.AccessModes |= inheritedAccessMode;
                }

                accessGroups.Add(newAccessGroup);
                AccessGroups = accessGroups;
            }

            //List<CFAggregation> childrenSetAccess = new List<CFAggregation>();

            foreach (CFAggregation child in ChildMembers)
            {
                if (!childrenSetAccess.Contains(child))
                {
                    childrenSetAccess.Add(child);
                    //child.SetAccess(guid, accessMode, true);
                    
                }
                
                //Db.Entry(child).State = System.Data.Entity.EntityState.Modified;
            }




            //if (accessGroup == null)
            //{
            //    base.SetAccess(guid, accessMode, isInherited);                
            //} else if (accessGroup.IsInherited == false && isInherited == true)
            //{
            //    return;
            //} else
            //{
            //    List<CFAccessGroup> accessGroups = AccessGroups;
            //    accessGroups.Remove(accessGroup);
            //    CFAccessGroup newAccessGroup = new CFAccessGroup();
            //    newAccessGroup.IsInherited = isInherited;
            //    newAccessGroup.Guid = guid.ToString();
            //    newAccessGroup.AccessDefinition.AccessModes = accessMode;

            //    if (accessGroup.IsInherited == true && isInherited == true)
            //    {
            //        AccessMode inheritedAccessMode = RecalculateInheritedPermissions(guid);
            //        newAccessGroup.AccessDefinition.AccessModes |= inheritedAccessMode;
            //    }

            //    accessGroups.Add(newAccessGroup);
            //    AccessGroups = accessGroups;

            //}

            //foreach (CFAggregation child in ChildMembers)
            //{
            //    child.SetAccess(guid, accessMode, true);
            //    //Db.Entry(child).State = System.Data.Entity.EntityState.Modified;
            //}
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
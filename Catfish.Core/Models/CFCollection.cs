using Catfish.Core.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Catfish.Core.Models
{
    [Serializable]
    public class CFCollection : CFAggregation
    {
        public override string GetTagName() { return "collection"; }
        public override string Label => "Collection";

        
        [NotMapped]
        [Display(Name = "Is System Collection")]
        public bool IsSystemCollection
        {
            get
            {
                var isSysColl = Data.Attribute("issystemcollection");
                if (isSysColl == null)
                {
                    Data.SetAttributeValue("issystemcollection", false);
                    return false;
                }
                else {
                    return Convert.ToBoolean(isSysColl.Value) ? true : false;
                }
            }

            set
            {
                Data.SetAttributeValue("issystemcollection", value);
            }
        }


        [NotMapped]
        [IgnoreDataMember]
        public virtual IEnumerable<CFAggregation> ChildCollections
        {
            get
            {
                return ChildMembers.Where(c => typeof(CFCollection).IsAssignableFrom(c.GetType()));
            }
        }
    }
}
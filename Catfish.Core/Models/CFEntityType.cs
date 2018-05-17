using System.Collections.Generic;
using Catfish.Core.Models.Forms;
using Catfish.Core.Models.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Runtime.Serialization;

namespace Catfish.Core.Models
{
    [Serializable]
    [CFTypeLabel("Entity Type")]
    public class CFEntityType
    {
        public enum eTarget { None = 0, Collections, Items, Files, Forms };

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        [NotMapped]
        public IList<eTarget> TargetTypesList {
            get
            {
                if (!string.IsNullOrEmpty(TargetTypes))
                {
                    return TargetTypes.Split(',')
                        .Select(s => (eTarget)System.Enum.Parse(typeof(eTarget), s))
                        .ToList();
                }

                return new List<eTarget>();
            }

            set
            {
                if (value != null)
                {
                    TargetTypes = string.Join(",", value.ToArray());
                }
            }
        }

        //public eTarget TargetType { get; set; }
        public string TargetTypes {get; set; } // MR: jan 15 2018, change to string, this will hold a comma separated value of TargetType

        [JsonIgnore] //Ignore this in JSON serialization to avoid stuck in a continuous loop
        [IgnoreDataMember]
        public virtual ICollection<CFMetadataSet> MetadataSets { get; set; }

        [JsonIgnore] //Ignore this in JSON serialization to avoid stuck in a continuous loop
        [IgnoreDataMember]
        public virtual ICollection<CFEntityTypeAttributeMapping> AttributeMappings { get; set; }

        [NotMapped]
        public bool HasAssociations { get { return false; } }

        public CFEntityType()
        {
            MetadataSets = new List<CFMetadataSet>();
            AttributeMappings = new List<CFEntityTypeAttributeMapping>();
            TargetTypesList = new List<eTarget>();
        }

        public CFEntityTypeAttributeMapping GetNameMapping()
        {
            return AttributeMappings.Where(mapping => mapping.Name == "Name Mapping").FirstOrDefault();
        }

        public CFEntityTypeAttributeMapping GetDescriptionMapping()
        {
            return AttributeMappings.Where(mapping => mapping.Name == "Description Mapping").FirstOrDefault();
        }
       
    }
}

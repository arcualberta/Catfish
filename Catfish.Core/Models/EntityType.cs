using System.Collections.Generic;
using Catfish.Core.Models.Forms;
using Catfish.Core.Models.Attributes;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using System.Linq;

namespace Catfish.Core.Models
{
    [TypeLabel("Entity Type")]
    public class EntityType
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
        public virtual ICollection<MetadataSet> MetadataSets { get; set; }

        [JsonIgnore] //Ignore this in JSON serialization to avoid stuck in a continuous loop
        public virtual ICollection<EntityTypeAttributeMapping> AttributeMappings { get; set; }

        [NotMapped]
        public bool HasAssociations { get { return false; } }

        public EntityType()
        {
            MetadataSets = new List<MetadataSet>();
            AttributeMappings = new List<EntityTypeAttributeMapping>();
            TargetTypesList = new List<eTarget>();
        }

        public EntityTypeAttributeMapping GetNameMapping()
        {
            return AttributeMappings.Where(mapping => mapping.Name == "Name Mapping").FirstOrDefault();
        }

        public EntityTypeAttributeMapping GetDescriptionMapping()
        {
            return AttributeMappings.Where(mapping => mapping.Name == "Description Mapping").FirstOrDefault();
        }
       
    }
}

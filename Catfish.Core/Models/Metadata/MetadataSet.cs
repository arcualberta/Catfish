using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Catfish.Core.Models.Attributes;
using System.Web.Script.Serialization;

namespace Catfish.Core.Models.Metadata
{
    [TypeLabel("Metadata Set")]
    public class MetadataSet
    {
        public int Id { get; set; }

        [TypeLabel("String")]
        public string Name { get; set; }

        [Rank(2)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual ICollection<SimpleField> Fields { get; set; }

        [ScriptIgnore(ApplyToOverrides = true)]
        public virtual ICollection<EntityType> EntityTypes { get; set; }

        public MetadataSet()
        {
            Fields = new List<SimpleField>();
        }
    }
}

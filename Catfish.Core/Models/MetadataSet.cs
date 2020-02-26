using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Catfish.Core.Models.Attributes;
//using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using Catfish.Core.Models.Forms;
using System;
using System.Runtime.Serialization;

namespace Catfish.Core.Models
{
    [Table("Catfish_MetadataSets")]
    [TypeLabel("Metadata Set")]
    [Serializable]
    public class MetadataSet : AbstractForm
    {
        public static string TagName
        {
            get
            {
                return "metadata-set";

            }
        }
        public override string GetTagName() { return TagName; }

        //[ScriptIgnore(ApplyToOverrides = true)] KR:.NETCORE
        [IgnoreDataMember]
        public virtual ICollection<EntityType> EntityTypes { get; set; }

        [NotMapped]
        public bool HasAssociations { get { return false; } }

    }
}

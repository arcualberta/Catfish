using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models
{
    public class AuditEntry:XmlModel
    {
        public enum eAction { Unknown = 0, Create, Update, Delete };

        public override string GetTagName() { return "entry"; }

        [NotMapped]
        public eAction Action
        {
            get
            {
                var att = Data.Attribute("action");
                if (att == null || string.IsNullOrEmpty(att.Value))
                    return eAction.Unknown;

                return (eAction)Enum.Parse(typeof(eAction), att.Value);
            }

            set
            {
                Data.SetAttributeValue("action", value);
            }
        }

        [NotMapped]
        public string User
        {
            get
            {
                var att = Data.Attribute("user");
                return att == null ? "" : att.Value;
            }

            set
            {
                Data.SetAttributeValue("user", value);
            }
        }

        [NotMapped]
        public string Target
        {
            get
            {
                var att = Data.Attribute("target");
                return att == null ? "" : att.Value;
            }

            set
            {
                Data.SetAttributeValue("target", value);
            }
        }

    }
}

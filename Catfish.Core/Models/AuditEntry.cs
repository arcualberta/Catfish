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
        public enum eAction { Other = 0, Create, Update, Delete, Grant, Revoke };

        public override string GetTagName() { return "entry"; }

        [NotMapped]
        public eAction Action
        {
            get
            {
                var att = Data.Attribute("action");
                if (att == null || string.IsNullOrEmpty(att.Value))
                    return eAction.Other;

                return (eAction)Enum.Parse(typeof(eAction), att.Value);
            }

            set
            {
                Data.SetAttributeValue("action", value);
            }
        }

        [NotMapped]
        public string Actor
        {
            get
            {
                var att = Data.Attribute("actor");
                return att == null ? "" : att.Value;
            }

            set
            {
                Data.SetAttributeValue("actor", value);
            }
        }

        public AuditEntry()
        {

        }

        public AuditEntry(eAction action, string actor, List<AuditChangeLog> changes)
        {
            Action = action;
            Actor = actor;
        }

        public void AppendLog(List<AuditChangeLog> changes)
        {
            foreach (var change in changes)
                Data.Add(change.ToXml());
        }

        public IReadOnlyList<AuditChangeLog> GetChangeLog()
        {
            return Data.Elements("log").Select(e => new AuditChangeLog(e)).ToList();
        }
    }
}

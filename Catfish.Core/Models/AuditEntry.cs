using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    [NotMapped]
    public class AuditEntry
    {
        public enum eAction { Other = 0, Create, Update, Delete, Grant, Revoke };

        public static readonly string ANNONYMOUS = "Annonymous";

        public XElement Data { get; private set; }

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

        public string Actor
        {
            get
            {
                var att = Data.Attribute("actor");
                return att == null ? ANNONYMOUS : att.Value;
            }

            set
            {
                Data.SetAttributeValue("actor", value);
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return DateTime.Parse(Data.Attribute("timestamp").Value);
            }

            set
            {
                Data.SetAttributeValue("timestamp", value);
            }
        }

        public AuditEntry(XElement data)
        {
            Data = data;
        }

        public AuditEntry(eAction action, string actor, DateTime timestamp, List<AuditChangeLog> changes)
        {
            Data = new XElement("entry");
            Action = action;
            Actor = actor;
            Timestamp = timestamp;
            AppendLog(changes);
        }

        public void AppendLog(List<AuditChangeLog> changes)
        {
            foreach (var change in changes)
            {
                XElement log = new XElement("log");
                log.SetAttributeValue("target", change.Target);
                log.Value = change.Description;
                Data.Add(log);
            }
        }

        public IReadOnlyList<AuditChangeLog> GetChangeLog()
        {
            return Data.Elements("log").Select(e => new AuditChangeLog(e.Attribute("target").Value, e.Value)).ToList();
        }
    }
}

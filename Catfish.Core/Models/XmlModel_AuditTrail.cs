using Catfish.Core.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Catfish.Core.Models
{
    public abstract partial class XmlModel
    {
        [NonSerialized]
        private List<AuditChangeLog> mChangeLog;
        public void LogChange(object target, string description)
        {
            mChangeLog.Add(new AuditChangeLog(target.ToString(), description));
        }

        protected XElement GetAuditRoot()
        {
            XElement audit = Data.Element("audit");
            if (audit == null)
                Data.Add(audit = new XElement("audit"));
            return audit;
        }

        /// <summary>
        /// Creates an audit entry and adds the change log into it.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="actor"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public AuditEntry FlushChangeLog(AuditEntry.eAction action, string actor, DateTime? timestamp = null)
        {
            AuditEntry entry = new AuditEntry(action, actor, timestamp.HasValue ? timestamp.Value : DateTime.Now, mChangeLog);
            AddAuditEntry(entry);
            mChangeLog.Clear();
            return entry;
        }

        public void AddAuditEntry(AuditEntry entry)
        {
            GetAuditRoot().Add(entry.Data);
        }
        public IEnumerable<AuditEntry> GetAuditTrail()
        {
            return GetAuditRoot().Elements("entry").Select(e => new AuditEntry(e));
        }

        public string GetCreator()
        {
            string xpath = "audit/entry[@action='" + AuditEntry.eAction.Create.ToString() + "']";
            XElement ele = GetChildElements(xpath, Data).FirstOrDefault();
            return ele == null ? null : ele.Attribute("user").Value;
        }

        ////public void Dispose()
        ////{
        ////    if(mData != null)
        ////    {
        ////        mData = null;
        ////    }
        ////}
    }
}

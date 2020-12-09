using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class AuditTrail : FieldContainer
    {
        public static readonly string TagName = "audit-trail";
        public static readonly string UserAtt = "user-id";
        public static readonly string FromAtt = "status-from";
        public static readonly string ToAtt = "status-to";
        public static readonly string TimeStampAtt = "timestamp";


        public Guid? UserId
        {
            get => GetAttribute(UserAtt, null as Guid?);
            set => SetAttribute(UserAtt, value);
        }

        public Guid StatusFrom
        {
            get => Guid.Parse(Data.Attribute(FromAtt).Value);
            set => SetAttribute(FromAtt, value);
        }

        public Guid StatusTo
        {
            get => Guid.Parse(Data.Attribute(ToAtt).Value);
            set => SetAttribute(ToAtt, value);
        }
        public DateTime TimeStamp
        {
            get => GetDateTimeAttribute(TimeStampAtt).Value;
            set => Data.SetAttributeValue(TimeStampAtt, value);
        }

        public AuditTrail() : base(TagName) { Initialize(eGuidOption.Ensure); TimeStamp = DateTime.Now; }
        public AuditTrail(XElement data) : base(data) { Initialize(eGuidOption.Ensure); TimeStamp = DateTime.Now; }

        
        public new void Initialize(eGuidOption guidOption)
        {
            //Ensuring that each metadata set has a unique ID
            base.Initialize(guidOption == eGuidOption.Ignore ? eGuidOption.Ensure : guidOption);
        }

        public DateTime? GetDateTimeAttribute(string key)
        {
            var att = Data.Attribute(key);
            return (att == null || string.IsNullOrEmpty(att.Value)) ? null as DateTime? : DateTime.Parse(att.Value);
        }
    }
}

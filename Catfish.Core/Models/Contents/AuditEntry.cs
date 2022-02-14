using Catfish.Core.Models.Contents.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class AuditEntry : FieldContainer
    {
        public static readonly string TagName = "audit-entry";
        public static readonly string UserAtt = "user-id";
        public static readonly string FromAtt = "status-from";
        public static readonly string ToAtt = "status-to";
        public static readonly string ActionAtt = "action";
        public static readonly string TimeStampAtt = "timestamp";
        public static readonly string ContentRootTag = "content";

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
        public string Action
        {
            get => GetAttribute(ActionAtt, null as string);
            set => SetAttribute(ActionAtt, value);
        }
        [NotMapped]
        public XmlModelList<XmlModel> Content { get; protected set; }

        public DateTime TimeStamp
        {
            get => GetDateTimeAttribute(TimeStampAtt).Value;
            set => Data.SetAttributeValue(TimeStampAtt, value);
        }

        public AuditEntry() : base(TagName) { Initialize(eGuidOption.Ensure); TimeStamp = DateTime.Now; }
        public AuditEntry(XElement data) : base(data) { Initialize(eGuidOption.Ensure); TimeStamp = DateTime.Now; }

        
        public new void Initialize(eGuidOption guidOption)
        {
            if (Data == null)
                Data = new XElement(TagName);

            //Wrapping the XElement "Data" in an XmlModel wrapper so that it can be used by the
            //rest of this initialization routine.
            XmlModel xml = new XmlModel(Data);

            //Ensuring that each metadata set has a unique ID
            base.Initialize(guidOption == eGuidOption.Ignore ? eGuidOption.Ensure : guidOption);
            //Building the DataContainer
            Content = new XmlModelList<XmlModel>(xml.GetElement(ContentRootTag, true), true);
        }

        public DateTime? GetDateTimeAttribute(string key)
        {
            var att = Data.Attribute(key);
            return (att == null || string.IsNullOrEmpty(att.Value)) ? null as DateTime? : DateTime.Parse(att.Value);
        }
    }
}

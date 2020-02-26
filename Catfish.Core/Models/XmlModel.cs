using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    [Table("Catfish_XmlModel")]
    public abstract partial class XmlModel
    {
        public abstract string GetTagName();
        public int Id { get; set; }

        [Column(TypeName = "xml")]
        public string Content
        {
            get => Data?.ToString();
            set => Data = string.IsNullOrEmpty(value) ? null : XElement.Parse(value); 
        }

        [NotMapped]
        public virtual XElement Data { get; set; }
        
        ////[Column(TypeName = "uniqueidentifier")]
        public Guid Guid
        {
            get => Guid.Parse(Data.Attribute("guid").Value); 
            set => Data.SetAttributeValue("guid", value); 
        }

        public DateTime Created
        {
            get => DateTime.Parse(Data.Attribute("created").Value); 
            set => Data.SetAttributeValue("created", value); 
        }

        public DateTime? Updated
        {
            get { try { return DateTime.Parse(Data.Attribute("updated").Value); } catch (Exception) { return null as DateTime?; } }
            set => Data.SetAttributeValue("updated", value);
        }

        public static Action<XmlModel> InitializeExternally = (m) => { };

        public XmlModel()
        {
            Data = new XElement(GetTagName());
            Guid = Guid.NewGuid();
            Created = DateTime.Now;
            Data.SetAttributeValue("model-type", this.GetType().AssemblyQualifiedName);
            Data.SetAttributeValue("is-required", false);
            mChangeLog = new List<AuditChangeLog>();

            InitializeExternally(this);
        }
    }
}
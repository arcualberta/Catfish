using Catfish.Core.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace Catfish.Core.Models.Forms
{
    [Obsolete]
    [TypeLabel("Metadata Definition")]
    public class MetadataDefinition
    {
        [XmlIgnore]
        public int Id { get; set; }

        public virtual List<FormField> Fields { get; set; }

        public MetadataDefinition()
        {
            Fields = new List<FormField>();
        }

        private XmlModel Data;

        public MetadataDefinition(XmlModel data, int id)
        {
            Id = id;
            Data = data;
        }

        ////////public override XElement ToXml()
        ////////{
        ////////    XElement ele = base.ToXml();

        ////////    XElement fieldEnvelope = new XElement("Fields");

        ////////    foreach (var field in Fields)
        ////////        fieldEnvelope.Add(field.ToXml());

        ////////    ele.Add(fieldEnvelope);

        ////////    return ele;
        ////////}
        ////////public override void Initialize(XElement ele)
        ////////{
        ////////    base.Initialize(ele);

        ////////    var fields = ele.Element("Fields").Elements();
        ////////    foreach(var xml in fields)
        ////////    {
        ////////        FormField field = XmlModel.Parse(xml) as FormField;
        ////////        this.Fields.Add(field);
        ////////    }
        ////////}
    }
}

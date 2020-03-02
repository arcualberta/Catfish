using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class FieldContainer : XmlModel
    {
        public XmlModelList<AbstractField> Fields { get; set; }
        public override string GetTagName() => "fields";

        public FieldContainer() { }
        public FieldContainer(XElement data) : base(data)
        {
        }
        public override void Initialize()
        {
            Fields = new XmlModelList<AbstractField>(Data, true, AbstractField.FieldTagName);
        }
    }
}

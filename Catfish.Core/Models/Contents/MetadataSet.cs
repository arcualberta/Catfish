using Catfish.Core.Models.Contents.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class MetadataSet : FieldContainer
    {
        public const string FieldContainerTag = "fields";

        public override string GetTagName() => "metadata-set";

        public string Name
        {
            get => Data.Element("name").Value;
            set => GetElement("name", true).Value = value;
        }

        public string Description
        {
            get => Data.Element("description").Value;
            set => GetElement("description", true).Value = value;
        }

        public MetadataSet() 
        { 
            Data = new XElement(GetTagName());
            Fields = new XmlModelList<AbstractField>(GetElement(FieldContainerTag, true));
        }
        public MetadataSet(XElement data) : base(data) { }
    }
}

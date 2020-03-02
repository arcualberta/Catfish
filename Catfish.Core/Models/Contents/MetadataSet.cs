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
        public const string TagName = "metadata-set";
        public MultilingualElement Name { get; protected set; }
        public MultilingualElement Description { get; protected set; }

        public MetadataSet() : base(TagName) { Initialize(); }
        public MetadataSet(XElement data) : base(data) { Initialize(); }

        public new void Initialize()
        {
            Name = new MultilingualElement(GetElement("name", true));
            Description = new MultilingualElement(GetElement("description", true));
        }
    }
}

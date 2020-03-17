using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.ViewModels
{
    public class EntityVM : XmlModel
    {
        public const string MetadataSetContainerTag = "metadata-set-container";
        public XmlModelList<MetadataSet> MetadataSets { get; protected set; }
        public EntityVM(string tagName) : base(tagName) { Initialize(); }

        public EntityVM(XElement data) : base(data) { Initialize(); }

        public void Initialize()
        {
            MetadataSets = new XmlModelList<MetadataSet>(GetElement(MetadataSetContainerTag, true), true, MetadataSet.TagName);
        }
    }
}

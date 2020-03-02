using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.ViewModels
{
    public abstract class EntityVM : XmlModel
    {
        public const string MetadataSetContainerTag = "metadata-set-container";
        public List<MetadataSet> MetadataSets { get; protected set; }
        public EntityVM() { }

        public EntityVM(XElement data) : base(data) { }

        public override void Initialize()
        {
            InitMetadataSets();
        }

        public void InitMetadataSets()
        {
            MetadataSets = new List<MetadataSet>();
            XElement container = GetElement(MetadataSetContainerTag, true);

            foreach(XElement ele in container.Elements())
            {
                MetadataSet ms = InstantiateContentModel(ele) as MetadataSet;
                MetadataSets.Add(ms);
            }
        }

        public void AppendMetadataSet(MetadataSet ms)
        {
            XElement container = GetElement(MetadataSetContainerTag, true);
            MetadataSets.Add(ms);
            container.Add(ms.Data);
        }
    }
}

using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class MetadataSet : FieldContainer
    {
        public static readonly string TagName = "metadata-set";
        public MetadataSet() : base(TagName) { Initialize(eGuidOption.Ensure); }
        public MetadataSet(XElement data) : base(data) { Initialize(eGuidOption.Ensure); }
    }
}

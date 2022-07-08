using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class MetadataSet : FieldContainer
    {
        public static readonly string TagName = "metadata-set";
        public MetadataSet() : base(TagName) { Initialize(eGuidOption.Ensure); }
        public MetadataSet(XElement data) : base(data) { Initialize(eGuidOption.Ensure); }

        public bool IsTemplate
        {
            get => GetAttribute("is-template", false);
            set => SetAttribute("is-template", value);
        }

        public new void Initialize(eGuidOption guidOption)
        {
            //Ensuring that each metadata set has a unique ID
            base.Initialize(guidOption == eGuidOption.Ignore ? eGuidOption.Ensure : guidOption);
        }
    }
}

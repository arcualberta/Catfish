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
        public MultilingualText Name { get; protected set; }
        public MultilingualText Description { get; protected set; }

        public MetadataSet() : base(TagName) { Initialize(eGuidOption.Ensure); }
        public MetadataSet(XElement data) : base(data) { Initialize(eGuidOption.Ensure); }

        public new void Initialize(eGuidOption guidOption)
        {
            //Ensuring that each metadata set has a unique ID
            base.Initialize(guidOption == eGuidOption.Ignore ? eGuidOption.Ensure : guidOption);

            Name = new MultilingualText(GetElement(Entity.NameTag, true));
            Description = new MultilingualText(GetElement(Entity.DescriptionTag, true));
        }

        public string GetName(string lang)
        {
            Text val = Name.Values.Where(val => val.Language == lang).FirstOrDefault();
            return val != null ? val.Value : null;
        }

        public void SetName(string metadataSetName, string lang)
        {
            Name.SetContent(metadataSetName, lang);
        }
    }
}

using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class Form: FieldContainer
    {
        public static readonly string TagName = "form";
        public Form() : base(TagName) { Initialize(eGuidOption.Ensure); }
        public Form(XElement data) : base(data) { Initialize(eGuidOption.Ensure); }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents.Fields
{
    public class InfoSection : BaseField
    {
        public static readonly string ContentTag = "content";

        public MultilingualText Content { get; set; }
        public InfoSection() { }
        public InfoSection(XElement data) : base(data) { }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            Content = new MultilingualText(GetElement(ContentTag, true));
        }

        public void SetContent(string content, string lang)
        {
            Content.SetContent(content, lang);
        }

        public string GetContent(string lang)
        {
            return Content.GetContent(lang);
        }

    }
}

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
        public InfoSection() { DisplayLabel = "Info Section"; }
        public InfoSection(XElement data) : base(data) { DisplayLabel = "Info Section"; }

        public override void Initialize(eGuidOption guidOption)
        {
            base.Initialize(guidOption);

            //Building the values
            Content = new MultilingualText(GetElement(ContentTag, true));
        }

        public void SetContent(string content, string lang, bool append = false)
        {
            Content.SetContent(content, lang);
        }

        public string GetContent(string lang)
        {
            return Content.GetContent(lang);
        }

        public override void UpdateValues(BaseField srcField)
        {
            //InforSection represents only display text and it does not 
            //accept any data through form submissions. Therefore, this method
            //does not need any implementation.
        }

        public InfoSection AppendContent(string htmlElementTag, string content, string lang, string htmlClasses = null)
        {
            if (htmlClasses == null)
                htmlClasses = "";
            string formattedContent = string.Format("<{0} class='{1}'>{2}</{0}>", htmlElementTag, htmlClasses, content);
            Content.AppendElement(formattedContent, lang);
            return this;
        }
    }
}

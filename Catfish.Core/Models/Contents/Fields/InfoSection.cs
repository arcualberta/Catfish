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

        public bool EditorOnly
        {
            get => GetAttribute("editor-only", false);
            set => SetAttribute("editor-only", value);
        }


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
            InfoSection src = srcField as InfoSection;

            Content.Values.Clear();
            foreach(var txt in src.Content.Values)
                Content.Values.Add(txt.Clone() as Text);
        }

        //This is a short-cut for setting the contents of the info field
        public override void SetValue(string value, string lang)
        {
            Content.SetContent(value, lang);
        }

        public InfoSection AppendContent(string htmlElementTag, string content, string lang, string htmlClasses = null)
        {
            if (htmlClasses == null)
                htmlClasses = "";
            string formattedContent = string.Format("<{0} class='{1}'>{2}</{0}>", htmlElementTag, htmlClasses, content);
            Content.SetContent(formattedContent, lang, true);
            return this;
        }

        /// <summary>
        /// This method is not relevant for the InfoSection field.
        /// </summary>
        /// <param name="srcField"></param>
        public override void CopyValue(BaseField srcField, bool overwrite = false) { }
    }
}

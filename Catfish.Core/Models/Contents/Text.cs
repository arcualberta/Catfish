using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class Text : XmlModel
    {
        public enum eFormat { plain, rich, css, javascript }
        public override string GetTagName() => "text";

        public override void Initialize()
        {
        }

        /// <summary>
        /// Override the default XElement creator method in XmlModel because
        /// the it does not set the language attribute.
        /// </summary>
        /// <param name="tagName"></param>
        public override void Initialize(string tagName)
        {
        }

        public eFormat Format
        {
            get => (eFormat)Enum.Parse(typeof(eFormat), Data.Attribute("format").Value);
            set => Data.SetAttributeValue("format", value);
        }

        public string Language => Data.Attribute("lang").Value;

        public int Rank
        {
            get => int.Parse(Data.Attribute("rank").Value);
            set => Data.SetAttributeValue("rank", value);
        }

        public string Value => Data.Value;

        public Text() { }
        public Text(XElement data) : base(data) { }
        public Text(string value, string lang)
        {
            XName ns = XNamespace.Xml + "lang";
            if (Data == null)
            {
                Data = new XElement(GetTagName(), new XAttribute(XNamespace.Xml + "lang", lang));
                UpdateModelType();
            }

            Data.Value = value;
        }
    }
}

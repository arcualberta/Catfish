using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class Text : XmlModel
    {
        public const string TagName = "text";
        public enum eFormat { plain, rich, css, javascript }

        /////// <summary>
        /////// Override the default XElement creator method in XmlModel because
        /////// the it does not set the language attribute.
        /////// </summary>
        /////// <param name="tagName"></param>
        ////public override void Initialize(string tagName)
        ////{
        ////}

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

        public Text() : base(TagName) { }
        public Text(XElement data) : base(data) { }
        public Text(string value, string lang) : base(TagName)
        {
            XAttribute att = Data.Attribute(XNamespace.Xml + "lang");
            if (att == null)
                att = new XAttribute(XNamespace.Xml + "lang", lang);
            else
                att.Value = lang;

            Data.Value = value;
        }
    }
}

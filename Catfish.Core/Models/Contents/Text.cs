using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Catfish.Core.Models.Contents
{
    public class Text : XmlModel
    {
        public const string TagName = "text";
        public enum eFormat {
            [EnumMember(Value = "plain")]
            plain,
            [EnumMember(Value = "rich-text")] 
            rich,
            [EnumMember(Value = "css")] 
            css,
            [EnumMember(Value = "javascript")] 
            javascript 
        }

        /////// <summary>
        /////// Override the default XElement creator method in XmlModel because
        /////// the it does not set the language attribute.
        /////// </summary>
        /////// <param name="tagName"></param>
        ////public override void Initialize(string tagName)
        ////{
        ////}

        [JsonConverter(typeof(StringEnumConverter))]
        public eFormat Format
        {
            get => Data.Attribute("format") == null ? eFormat.plain : (eFormat)Enum.Parse(typeof(eFormat), Data.Attribute("format").Value);
            set => Data.SetAttributeValue("format", value);
        }

        public string Language
        {
            get => Data.Attribute(XNamespace.Xml + "lang")?.Value;
            set => Data.SetAttributeValue(XNamespace.Xml + "lang", value);
        }

        public int Rank
        {
            get => Data.Attribute("rank") == null ? 0 : int.Parse(Data.Attribute("rank").Value);
            set => Data.SetAttributeValue("rank", value);
        }

        public string Value
        {
            get => Data.Value;
            set => Data.Value = value == null ? "" : value;
        }

        [JsonIgnore]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateValue
        {
            get => string.IsNullOrEmpty(Data.Value) || Data.Value.StartsWith("0001") ? null as DateTime? : DateTime.Parse(Data.Value);
            set => Data.Value = value.HasValue ? value.Value.ToString() : "";
        }

        [JsonIgnore]
        public int IntValue
        {
            get => string.IsNullOrEmpty(Data.Value) ? 0 : int.Parse(Data.Value);
            set => Data.Value = value.ToString();
        }

        [JsonIgnore]
        public decimal DecimalValue
        {
            get => string.IsNullOrEmpty(Data.Value) ? 0 : decimal.Parse(Data.Value);
            set => Data.Value = value.ToString();
        }

        public Text() : base(TagName) { SetNewGuid(); }
        public Text(XElement data) : base(data) { }
        public Text(string value, string lang) : base(TagName)
        {
            Data.SetAttributeValue(XNamespace.Xml + "lang", lang);
            if (value != null)
                Data.Value = value;
            SetNewGuid();
        }

        public Text(Text src)
             : base(TagName)
        {
            SetNewGuid();
            Copy(src);
        }

        public void Copy(Text src)
        {
            Value = src.Value;
            Language = src.Language;
            Format = src.Format;
            Rank = src.Rank;
        }
    }
}

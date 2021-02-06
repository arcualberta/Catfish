using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class FileReference : XmlModel
    {
        public const string TagName = "file";

        public string Path
        {
            get => GetAttribute("path", null as string);
            set => SetAttribute("path", value);
        }

        public string Url
        {
            get => GetAttribute("url", null as string);
            set => SetAttribute("url", value);
        }
        public string Thumbnail
        {
            get => GetAttribute("thumbnail", null as string);
            set => SetAttribute("thumbnail", value);
        }

        public string ContentType
        {
            get => GetAttribute("content-type", null as string);
            set => SetAttribute("content-type", value);
        }

        public long Size
        {
            get => GetAttribute("size", 0);
            set => SetAttribute("size", value);
        }


        public string OriginalName
        {
            get => GetAttribute("original-name", null as string);
            set => SetAttribute("original-name", value);
        }


        public FileReference() : base(TagName) { SetNewGuid(); }
        public FileReference(XElement data) : base(data) { }
        public FileReference(string value, string lang) : base(TagName)
        {
            Data.SetAttributeValue(XNamespace.Xml + "lang", lang);
            if (value != null)
                Data.Value = value;
            SetNewGuid();
        }
    }
}

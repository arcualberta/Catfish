using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    public class FileReference : XmlModel
    {
        public const string TagName = "file";

        public string FileName
        {
            get => GetAttribute("file", null as string);
            set => SetAttribute("file", value);
        }
        public string OriginalFileName
        {
            get => GetAttribute("original-file", null as string);
            set => SetAttribute("original-file", value);
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

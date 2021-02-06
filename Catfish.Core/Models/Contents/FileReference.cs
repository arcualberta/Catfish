using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace Catfish.Core.Models.Contents
{
    [Table("Catfish_Files")]
    public class FileReference : XmlModel
    {
        public enum eStatus { Temporary = 1, Permanent}
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

        public Guid? FieldId
        {
            get => GetAttribute("field-id", null as Guid?);
            set => SetAttribute("field-id", value);
        }
        public Guid? ItemId
        {
            get => GetAttribute("item-id", null as Guid?);
            set => SetAttribute("item-id", value);
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

        //No need to store this in the XML object but we need it in the database table
        //so that we can distinguish between permanent files and temporary files.
        public eStatus Status { get; set; }

        //No need to store this in the XML object or database. This should be computed
        //and initilized before the file is sent to the front end.
        [NotMapped]
        public string Url { get; set; }


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

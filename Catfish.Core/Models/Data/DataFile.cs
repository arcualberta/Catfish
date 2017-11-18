﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Data
{
    public class DataFile: DataObject
    {
        public enum eThumbnailTypes { NonShared = 0, Shared }

        public override string GetTagName() { return "file"; }

        [NotMapped]
        public string ContentType { get { return GetAttribute("content-type", null); } set { SetAttribute("content-type", value); } }

        [NotMapped]
        public string FileName { get { return GetAttribute("file-name", null); } set { SetAttribute("file-name", value); } }

        [NotMapped]
        public string GuidName { get { return GetAttribute("guid-name", null); } set { SetAttribute("guid-name", value); Guid = value; } }

        [NotMapped]
        public string Thumbnail { get { return GetAttribute("thumbnail", null); } set { SetAttribute("thumbnail", value); } }

        [NotMapped]
        public string Path { get { return GetAttribute("path", null); } set { SetAttribute("path", value); } }

        [NotMapped]
        public eThumbnailTypes ThumbnailType
        {
            get
            {
                try
                {
                    return (eThumbnailTypes)Enum.Parse(typeof(eThumbnailTypes), GetAttribute("thumbnail-type", null));
                }
                catch (Exception)
                {
                    return eThumbnailTypes.Shared;
                }
            }
            set
            {
                SetAttribute("thumbnail-type", value.ToString());
            }
        }

    }
}

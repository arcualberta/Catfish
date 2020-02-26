using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models.Forms
{
    [TypeLabel("External Media Field")]
    public class ExternalMediaField : FormField
    {
        [TypeLabel("Media Type")]
        [NotMapped]
        public DataFile.MimeType MediaType
        {
            get
            {
                Type eType = typeof(DataFile.MimeType);
                string attr = GetAttribute("media-type", Data) ?? Enum.GetName(typeof(DataFile.MimeType), DataFile.MimeType.Application);
                return (DataFile.MimeType)Enum.Parse(eType, attr);
            }

            set
            {
                SetAttribute("media-type", Enum.GetName(typeof(DataFile.MimeType), value));
            }
        }

        [TypeLabel("Media Location")]
        [NotMapped]
        public string Source
        {
            get
            {
                return GetAttribute("src", Data);
            }

            set
            {
                SetAttribute("src", value);
            }
        }
        
        [NotMapped]
        [MediaType("Play Once", DataFile.MimeType.Audio, DataFile.MimeType.Video)]
        public bool PlayOnce
        {
            get
            {
                return GetAttribute("play-once", false);
            }

            set
            {
                SetAttribute("play-once", value);
            }
        }
        
        [NotMapped]
        [MediaType("Play On Show", DataFile.MimeType.Audio, DataFile.MimeType.Video)]
        public bool PlayOnShow
        {
            get
            {
                return GetAttribute("play-on-show", false);
            }

            set
            {
                SetAttribute("play-on-show", value);
            }
        }
        
        [NotMapped]
        [MediaType("Show Controls", DataFile.MimeType.Audio, DataFile.MimeType.Video)]
        public bool ShowControls
        {
            get
            {
                return GetAttribute("show-controls", true);
            }

            set
            {
                SetAttribute("show-controls", value);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MediaTypeAttribute : Attribute 
    {
        public IEnumerable<DataFile.MimeType> MimeTypes { get; protected set; }
        public string Label { get; set; }

        public MediaTypeAttribute(string label, params DataFile.MimeType[] mimeTypes)
        {
            Label = label;
            MimeTypes = mimeTypes;
        }
    }
}

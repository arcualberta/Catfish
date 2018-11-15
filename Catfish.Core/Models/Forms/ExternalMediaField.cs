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
    [CFTypeLabel("External Media Field")]
    public class ExternalMediaField : FormField
    {
        [CFTypeLabel("Media Type")]
        [NotMapped]
        public CFDataFile.MimeType MediaType
        {
            get
            {
                Type eType = typeof(CFDataFile.MimeType);
                string attr = GetAttribute("media-type", Data) ?? Enum.GetName(typeof(CFDataFile.MimeType), CFDataFile.MimeType.Application);
                return (CFDataFile.MimeType)Enum.Parse(eType, attr);
            }

            set
            {
                SetAttribute("media-type", Enum.GetName(typeof(CFDataFile.MimeType), value));
            }
        }

        [CFTypeLabel("Media Location")]
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
        [MediaType("Play Once", CFDataFile.MimeType.Audio, CFDataFile.MimeType.Video)]
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
        [MediaType("Play On Show", CFDataFile.MimeType.Audio, CFDataFile.MimeType.Video)]
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
        [MediaType("Show Controls", CFDataFile.MimeType.Audio, CFDataFile.MimeType.Video)]
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
        public IEnumerable<CFDataFile.MimeType> MimeTypes { get; protected set; }
        public string Label { get; set; }

        public MediaTypeAttribute(string label, params CFDataFile.MimeType[] mimeTypes)
        {
            Label = label;
            MimeTypes = mimeTypes;
        }
    }
}

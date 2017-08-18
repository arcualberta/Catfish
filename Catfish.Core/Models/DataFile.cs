using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    public class DataFile: Entity
    {
        [NotMapped]
        public string FileName { get { return GetAttribute("file-name"); } set { SetAttribute("file-name", value); } }

        [NotMapped]
        public string Url { get { return GetAttribute("url"); } set { SetAttribute("url", value); } }

        [NotMapped]
        public string Thumbnail { get { return GetAttribute("thumbnail"); } set { SetAttribute("thumbnail", value); } }

        [NotMapped]
        public eThumbnailTypes ThumbnailType
        {
            get
            {
                try
                {
                    return (eThumbnailTypes)Enum.Parse(typeof(eThumbnailTypes), GetAttribute("thumbnail-type"));
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

        public enum eThumbnailTypes { NonShared = 0, Shared }

        public override string GetTagName() { return "file"; }

    }
}

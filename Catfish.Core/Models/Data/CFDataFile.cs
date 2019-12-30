using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models.Data
{
    public class CFDataFile: CFDataObject
    {
        public enum MimeType { Text, Image, Audio, Video, Application };

        public static string TagName { get { return "file"; } }
        public enum eThumbnailTypes { NonShared = 0, Shared }

      

        public override string GetTagName() { return TagName; }

        [NotMapped]
        public MimeType TopMimeType
        {
            get
            {                
                string[] contentTypeParts = ContentType.Split('/');

                if (contentTypeParts.Count() > 0)
                {
                    switch(contentTypeParts[0])
                    {
                        case "image" : return MimeType.Image;
                        case "audio" : return MimeType.Audio;
                        case "video": return MimeType.Video;
                        case "application": return MimeType.Application;
                    }
                }
                return MimeType.Text;                
            }
        }

        [NotMapped]
        public string ContentType { get { return GetAttribute("content-type", null); } set { SetAttribute("content-type", value); } }

        [NotMapped]
        public string FileName { get { return GetAttribute("file-name", null); } set { SetAttribute("file-name", value); } }

        [NotMapped]
        public string Extension
        {
            get
            {
                string filename = FileName;
                int idx = filename.LastIndexOf('.');
                if (idx < 0)
                    return null;
                else
                    return filename.Substring(idx + 1);

            }
        }

        [NotMapped]
        public string LocalFileName { get { return Guid + "." + Extension; } }


        [NotMapped]
        public string Thumbnail { get { return GetAttribute("thumbnail", null); } set { SetAttribute("thumbnail", value); } }
        [NotMapped]
        public string Small { get { return GetAttribute("small", null); } set { SetAttribute("small", value); } }
        [NotMapped]
        public string Medium { get { return GetAttribute("medium", null); } set { SetAttribute("medium", value); } }
        [NotMapped]
        public string Large { get { return GetAttribute("large", null); } set { SetAttribute("large", value); } }

        [NotMapped]
        public string Path
        {
            get
            {
                string relativePath = GetAttribute("path", null);
                if (relativePath == null)
                    return null;
                return System.IO.Path.Combine(ConfigHelper.UploadRoot, relativePath);
            }
            set
            {
                string path = (value != null && value.StartsWith(ConfigHelper.UploadRoot)) ?
                    value.Substring(ConfigHelper.UploadRoot.Length + 1) :
                    value;

                SetAttribute("path", path);
            }
        }

        [NotMapped]
        public string AbsoluteFilePathName { get { return System.IO.Path.Combine(Path, LocalFileName); } }

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

        public void DeleteFilesFromFileSystem()
        {
            //Deleting the file from the file system
            File.Delete(AbsoluteFilePathName);

            //If the thumbnail is not a shared one, deleting it as well from the file system
            if (ThumbnailType != CFDataFile.eThumbnailTypes.Shared)
                File.Delete(System.IO.Path.Combine(ConfigHelper.UploadRoot, Thumbnail));
        }

    }
}

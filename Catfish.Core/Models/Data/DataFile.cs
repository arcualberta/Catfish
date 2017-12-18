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
    public class DataFile: DataObject
    {
        public static string TagName { get { return "file"; } }
        public enum eThumbnailTypes { NonShared = 0, Shared }

        public override string GetTagName() { return TagName; }

        [NotMapped]
        public string ContentType { get { return GetAttribute("content-type", null); } set { SetAttribute("content-type", value); } }

        [NotMapped]
        public string FileName { get { return GetAttribute("file-name", null); } set { SetAttribute("file-name", value); } }

        [NotMapped]
        public string GuidName { get { return GetAttribute("guid-name", null); } set { SetAttribute("guid-name", value); Guid = value; } }

        [NotMapped]
        public string Thumbnail { get { return GetAttribute("thumbnail", null); } set { SetAttribute("thumbnail", value); } }

        [NotMapped]
        public string Path
        {
            get
            {
                string relativePath = GetAttribute("path", null);
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
        public string AbsoluteFilePathName { get { return System.IO.Path.Combine(Path, GuidName); } }

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
            if (ThumbnailType != DataFile.eThumbnailTypes.Shared)
                File.Delete(System.IO.Path.Combine(ConfigHelper.UploadRoot, Thumbnail));
        }

    }
}

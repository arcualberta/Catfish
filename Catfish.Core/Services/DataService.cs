using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Catfish.Core.Services
{
    public class DataService : ServiceBase
    {
        public DataService(CatfishDbContext db) : base(db)
        {

        }
        
        protected string CreateThumbnailName(string guid, string srcExtension)
        {
            string ext = "";
            ImageFormat format = GetThumbnailFormat(srcExtension);
            if (format == ImageFormat.Jpeg)
                ext = "jpg";
            else if (format == ImageFormat.Png)
                ext = "png";
            else
                throw new Exception("Unknown thumbnail format");

            string thumbnailFileName = guid + "_t." + ext;
            return thumbnailFileName;
        }

        protected string CreateVariatyImageSizeName(string guid, string srcExtension, string size)
        {
            string ext = "";
            ImageFormat format = GetThumbnailFormat(srcExtension);
            if (format == ImageFormat.Jpeg)
                ext = "jpg";
            else if (format == ImageFormat.Png)
                ext = "png";
            else
                throw new Exception("Unknown image format");

            string imageFileName = string.Empty;

            if(size.Equals(ConfigHelper.eImageSize.Small.ToString()))
                imageFileName = guid + "_s." + ext;
            else if(size.Equals(ConfigHelper.eImageSize.Medium.ToString()))
                imageFileName = guid + "_m." + ext;
            else //large
                imageFileName = guid + "_l." + ext;

            return imageFileName;
        }

        protected ImageFormat GetThumbnailFormat(string srcExtension)
        {
            //add .jpeg
            return (srcExtension == "jpg" || srcExtension == "jpeg") ? ImageFormat.Jpeg : ImageFormat.Png;
        }

        protected ImageFormat GetImageFormat(string srcExtension)
        {
            return (srcExtension == "jpg" || srcExtension == "jpeg") ? ImageFormat.Jpeg : ImageFormat.Png;
        }

        public List<CFDataFile> UploadTempFiles(HttpRequestBase request)
        {
            List<CFDataFile> files = UploadFiles(request, "temp-files");
            Db.XmlModels.AddRange(files);
            return files;
        }

        protected List<CFDataFile> UploadFiles(HttpRequestBase request, string dstPath)
        {
            dstPath = Path.Combine(ConfigHelper.UploadRoot, dstPath);
            if (!Directory.Exists(dstPath))
            {
                Directory.CreateDirectory(dstPath);
                if (!Directory.Exists(dstPath))
                    throw new Exception("Unable to create the upload folder " + dstPath);
            }

            List<CFDataFile> newFiles = new List<CFDataFile>();
            for (int i = 0; i < request.Files.Count; ++i)
                newFiles.Add(InjestFile(request.Files[i].InputStream, request.Files[i].FileName, request.Files[i].ContentType, dstPath));

            return newFiles;
        }



        public CFDataFile InjestFile(Stream srcStream, string inputFileName, string contentType, string dstPath)
        {
            dstPath = Path.Combine(ConfigHelper.UploadRoot, dstPath);
            if (!Directory.Exists(dstPath))
            {
                Directory.CreateDirectory(dstPath);
                if (!Directory.Exists(dstPath))
                    throw new Exception("Unable to create the upload folder " + dstPath);
            }

            CFDataFile file = new CFDataFile()
            {
                FileName = inputFileName,
                Path = dstPath,
                ContentType = contentType
            };

            using (FileStream dstFileStream = File.Create(Path.Combine(file.Path, file.LocalFileName)))
            {
                srcStream.Seek(0, SeekOrigin.Begin);
                srcStream.CopyTo(dstFileStream);
            }

            if (file.ContentType.StartsWith("image/"))
            {
                file.Thumbnail = CreateThumbnailName(file.Guid, file.Extension);
                file.ThumbnailType = CFDataFile.eThumbnailTypes.NonShared;

                //August 1 2018 -- create different size of image
                file.Small = CreateVariatyImageSizeName(file.Guid, file.Extension, "Small");
                file.Medium = CreateVariatyImageSizeName(file.Guid, file.Extension, "Medium");
                file.Large = CreateVariatyImageSizeName(file.Guid, file.Extension, "Large");

              //august 1 2018 -- adding different size of image, not just thumbnail
                using (Image image = new Bitmap(file.AbsoluteFilePathName))
                {
                    foreach (var enumValue in Enum.GetValues(typeof(ConfigHelper.eImageSize)))
                    {
                            int sizeVal = (int)enumValue;
                        Size imgSize = image.Width < image.Height
                       ? new Size() { Height = sizeVal, Width = (image.Width * sizeVal) / image.Height }
                       : new Size() { Width = sizeVal, Height = (image.Height * sizeVal) / image.Width };

                        Image img = image.GetThumbnailImage(imgSize.Width, imgSize.Height, null, IntPtr.Zero);
                        ImageFormat format = GetImageFormat(file.Extension);

                        if(enumValue.Equals(ConfigHelper.eImageSize.Thumbnail))
                            img.Save(Path.Combine(file.Path, file.Thumbnail), format);
                        else if (enumValue.Equals(ConfigHelper.eImageSize.Small))
                            img.Save(Path.Combine(file.Path, file.Small), format);
                        else if (enumValue.Equals(ConfigHelper.eImageSize.Medium))
                            img.Save(Path.Combine(file.Path, file.Medium), format);
                        else if (enumValue.Equals(ConfigHelper.eImageSize.Large))
                                img.Save(Path.Combine(file.Path, file.Large), format);
                    }
                   
                }
            }
            else
            {
                file.Thumbnail = GetThumbnail(file.ContentType);
                file.ThumbnailType = CFDataFile.eThumbnailTypes.Shared;
            }

            return file;
        }

        public CFDataFile GetFile(int id, string guid, bool checkInItems = true)
        {
            CFXmlModel model = Db.XmlModels.Find(id);

            if (model is CFDataFile && model.Guid == guid)
                return model as CFDataFile;

            if (checkInItems && model is CFItem)
                return (model as CFItem).Files.Where(f => f.Guid == guid).FirstOrDefault();

            if (typeof(AbstractForm).IsAssignableFrom(model.GetType()))            
                return (model as AbstractForm).Fields.SelectMany(m => m.Files).Where(m => m.DataFile.Guid == guid).FirstOrDefault().DataFile;

            return null;
        }

        public bool DeleteStandaloneFile(string guid)
        {
            CFDataFile file = Db.XmlModels.Where(x => x.MappedGuid == guid).FirstOrDefault() as CFDataFile;
            if (file == null)
                return false;

            //Deleting the file from the file system
            file.DeleteFilesFromFileSystem();

            //Deleting the file object from the database
            Db.XmlModels.Remove(file);

            return true;
        }

        public string GetThumbnail(string contentType)
        {
            if (contentType == "application/pdf")
                return "pdf.png";

            if (contentType == "application/msword")
                return "doc.png";

            if (contentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                return "docx.png";

            if (contentType == "application/vnd.ms-excel")
                return "xls.png";

            if (contentType == "application/x-zip-compressed")
                return "zip.png";

            if (contentType == "image/jpeg")
                return "jpg.png";

            if (contentType == "image/png")
                return "png.png";

            if (contentType == "image/tiff")
                return "tiff.png";

            if (contentType == "text/html")
                return "html.png";

            if (contentType == "text/xml")
                return "xml.png";

            return "other.png";
        }
    }
}

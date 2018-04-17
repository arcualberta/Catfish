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

        protected ImageFormat GetThumbnailFormat(string srcExtension)
        {
            return srcExtension == "jpg" ? ImageFormat.Jpeg : ImageFormat.Png;
        }
        
        public List<DataFile> UploadTempFiles(HttpRequestBase request)
        {
            List<DataFile> files = UploadFiles(request, "temp-files");
            Db.XmlModels.AddRange(files);
            return files;
        }

        protected List<DataFile> UploadFiles(HttpRequestBase request, string dstPath)
        {
            dstPath = Path.Combine(ConfigHelper.UploadRoot, dstPath);
            if (!Directory.Exists(dstPath))
            {
                Directory.CreateDirectory(dstPath);
                if (!Directory.Exists(dstPath))
                    throw new Exception("Unable to create the upload folder " + dstPath);
            }

            List<DataFile> newFiles = new List<DataFile>();
            for (int i = 0; i < request.Files.Count; ++i)
                newFiles.Add(InjestFile(request.Files[i].InputStream, request.Files[i].FileName, request.Files[i].ContentType, dstPath));

            return newFiles;
        }



        public DataFile InjestFile(Stream srcStream, string inputFileName, string contentType, string dstPath)
        {
            dstPath = Path.Combine(ConfigHelper.UploadRoot, dstPath);
            if (!Directory.Exists(dstPath))
            {
                Directory.CreateDirectory(dstPath);
                if (!Directory.Exists(dstPath))
                    throw new Exception("Unable to create the upload folder " + dstPath);
            }

            DataFile file = new DataFile()
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
                file.ThumbnailType = DataFile.eThumbnailTypes.NonShared;
                using (Image image = new Bitmap(file.AbsoluteFilePathName))
                {
                    Size thumbSize = image.Width < image.Height
                        ? new Size() { Height = ConfigHelper.ThumbnailSize, Width = (image.Width * ConfigHelper.ThumbnailSize) / image.Height }
                        : new Size() { Width = ConfigHelper.ThumbnailSize, Height = (image.Height * ConfigHelper.ThumbnailSize) / image.Width };

                    Image thumbnail = image.GetThumbnailImage(thumbSize.Width, thumbSize.Height, null, IntPtr.Zero);
                    ImageFormat format = GetThumbnailFormat(file.Extension);
                    thumbnail.Save(Path.Combine(file.Path, file.Thumbnail), format);
                }
            }
            else
            {
                file.Thumbnail = GetThumbnail(file.ContentType);
                file.ThumbnailType = DataFile.eThumbnailTypes.Shared;
            }

            return file;
        }

        public DataFile GetFile(int id, string guid, bool checkInItems = true)
        {
            XmlModel model = Db.XmlModels.Find(id);

            if (model is DataFile && model.Guid == guid)
                return model as DataFile;

            if (checkInItems && model is Item)
                return (model as Item).Files.Where(f => f.Guid == guid).FirstOrDefault();

            if (typeof(AbstractForm).IsAssignableFrom(model.GetType()))            
                return (model as AbstractForm).Fields.SelectMany(m => m.Files).Where(m => m.DataFile.Guid == guid).FirstOrDefault().DataFile;

            return null;
        }

        public bool DeleteStandaloneFile(string guid)
        {
            DataFile file = Db.XmlModels.Where(x => x.MappedGuid == guid).FirstOrDefault() as DataFile;
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

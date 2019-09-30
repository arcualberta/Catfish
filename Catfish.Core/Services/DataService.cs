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
            else if (format == ImageFormat.Gif)
                ext = "gif";
            else if (format == ImageFormat.Png || format == ImageFormat.Tiff)
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
            else if (format == ImageFormat.Gif)
                ext = "gif";
            else if (format == ImageFormat.Png || format == ImageFormat.Tiff)
                ext = "png";
            else
                throw new Exception("Unknown image format");

            string imageFileName = string.Empty;

            if (size.Equals(ConfigHelper.eImageSize.Small.ToString()))
                imageFileName = guid + "_s." + ext;
            else if (size.Equals(ConfigHelper.eImageSize.Medium.ToString()))
                imageFileName = guid + "_m." + ext;
            else if (size.Equals(ConfigHelper.eImageSize.Large.ToString()))
                imageFileName = guid + "_l." + ext;
            else
                imageFileName = guid + "_" + size + "." + ext;

            return imageFileName;
        }

        protected ImageFormat GetThumbnailFormat(string srcExtension)
        {
            //add .jpeg
            // return (srcExtension == "jpg" || srcExtension == "jpeg") ? ImageFormat.Jpeg : ImageFormat.Png;
            ImageFormat imgFormat = null;
            srcExtension = srcExtension.ToLower();
            if (srcExtension == "jpg" || srcExtension == "jpeg")
            {
                imgFormat = ImageFormat.Jpeg;
            }
            else if (srcExtension == "gif")
            {
                imgFormat = ImageFormat.Gif;
            }
            else if(srcExtension == "png" || srcExtension == "tif" || srcExtension == "tiff")
            {
                imgFormat =  ImageFormat.Png;
            }

            return imgFormat;
        }

        protected ImageFormat GetImageFormat(string srcExtension)
        {
            // return (srcExtension == "jpg" || srcExtension == "jpeg") ? ImageFormat.Jpeg : ImageFormat.Png;
            ImageFormat imgFormat = null;
            srcExtension = srcExtension.ToLower();
            if (srcExtension == "jpg" || srcExtension == "jpeg")
            {
                imgFormat = ImageFormat.Jpeg;
            }
            else if (srcExtension == "gif")
            {
                imgFormat = ImageFormat.Gif;
            }
            else if (srcExtension == "png" || srcExtension == "tif" || srcExtension == "tiff")
            {
                imgFormat = ImageFormat.Png;
            }

            return imgFormat;
        }

        public List<CFDataFile> UploadTempFiles(HttpRequestBase request, int maxPixelSize = 0)
        {
            List<CFDataFile> files = UploadFiles(request, "temp-files", maxPixelSize);
            Db.XmlModels.AddRange(files);
            return files;
        }

        protected List<CFDataFile> UploadFiles(HttpRequestBase request, string dstPath, int maxPixelSize)
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
                newFiles.Add(InjestFile(request.Files[i].InputStream, request.Files[i].FileName.ToLower(), request.Files[i].ContentType, dstPath, maxPixelSize));

            return newFiles;
        }


        private Image ResizeImage(BitmapData img, ColorPalette palette, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);

            Bitmap original = new Bitmap(img.Width, img.Height, img.Stride, img.PixelFormat, img.Scan0);

            if (palette != null && palette.Entries.Length > 0)
            {
                original.Palette = palette;
            }

            using(var graphic = Graphics.FromImage(result))
            {
                graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphic.DrawImage(original, 0, 0, width, height);
            }

            return result;
        }

        public CFDataFile InjestFile(Stream srcStream, string inputFileName, string contentType, string dstPath, int maxPixelSize)
        {
            dstPath = Path.Combine(ConfigHelper.UploadRoot, dstPath);
            if (!Directory.Exists(dstPath))
            {
                Directory.CreateDirectory(dstPath);
                if (!Directory.Exists(dstPath))
                    throw new Exception("Unable to create the upload folder " + dstPath);
            }

            //change the content type to image/png if the content type is image/tif
            contentType = contentType == "image/tiff" ? "image/png" : contentType;

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

                ImageFormat format = GetImageFormat(file.Extension);

                Action<string, BitmapData, ColorPalette, int, bool> saveImage = (path, image, palette, sizeVal, ignoreIfSmaller) =>
                {
                    int width = image.Width;
                    int height = image.Height;
                    
                    if (ignoreIfSmaller && width < sizeVal && height < sizeVal)
                    {
                        return;
                    }
                    
                    Size imgSize = width < height
                        ? new Size() { Height = sizeVal, Width = (width * sizeVal) / height }
                        : new Size() { Width = sizeVal, Height = (height * sizeVal) / width };

                    Image img = ResizeImage(image, palette, imgSize.Width, imgSize.Height);

                    img.Save(path, format);
                    img.Dispose();
                };

                // Add different image sizes.
                string resizedFilePath = null;
                using (Bitmap image = new Bitmap(file.AbsoluteFilePathName))
                {
                    var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
                    ColorPalette palette = image.Palette;

                    Parallel.Invoke(
                        () => saveImage(Path.Combine(file.Path, file.Thumbnail), data, palette, (int)ConfigHelper.eImageSize.Thumbnail, false),
                        () => saveImage(Path.Combine(file.Path, file.Small), data, palette, (int)ConfigHelper.eImageSize.Small, false),
                        () => saveImage(Path.Combine(file.Path, file.Medium), data, palette, (int)ConfigHelper.eImageSize.Medium, false),
                        () => saveImage(Path.Combine(file.Path, file.Large), data, palette, (int)ConfigHelper.eImageSize.Large, false)
                    );

                    if(maxPixelSize > 0)
                    {
                        resizedFilePath = Path.Combine(file.Path, CreateVariatyImageSizeName(file.Guid, file.Extension, "Resize"));
                        saveImage(resizedFilePath, data, palette, maxPixelSize, true);
                    }
                }

                if (resizedFilePath != null && File.Exists(resizedFilePath)) {
                    string path = Path.Combine(file.Path, file.LocalFileName);
                    File.Delete(path);
                    File.Move(resizedFilePath, path);
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

using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;

namespace Catfish.Core.Services
{
    public class ItemService: EntityService
    {
        public ItemService(CatfishDbContext db) : base(db) { }

        public string GetURL(string pathName)
        {
            var uri = new System.Uri(pathName);
            var converted = uri.AbsoluteUri;
            return converted;
        }

        protected string CreateGuidName(string baseName)
        {
            string filename = Guid.NewGuid().ToString().Replace("-", "");
            var idx = baseName.LastIndexOf(".");
            if (idx > 0)
                filename = filename + "." + baseName.Substring(idx + 1);
            return filename;
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
            for(int i=0; i<request.Files.Count; ++i)
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
            else if (checkInItems && model is Item)
                return (model as Item).Files.Where(f => f.Guid == guid).FirstOrDefault();
            else
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

        protected void UpdateFiles(Item srcItem, Item dstItem)
        {
            UpdateFiles(srcItem.AttachmentField, dstItem);
        }
        protected void UpdateFiles(Attachment srcAttachmentField, Item dstItem)
        {
            List<string> keepFileGuids = srcAttachmentField.FileGuids.Split(new char[] { Attachment.FileGuidSeparator }, StringSplitOptions.RemoveEmptyEntries).ToList();

            //Removing attachments that are in the dbModel but not in attachments to be kept
            foreach (DataFile file in dstItem.Files.ToList())
            {
                if (keepFileGuids.IndexOf(file.Guid) < 0)
                {
                    //Deleting the file node from the XML Model
                    dstItem.RemoveFile(file);
                }
            }

            //Adding new files
            foreach (string fileGuid in keepFileGuids)
            {
                if (dstItem.Files.Where(f => f.Guid == fileGuid).Any() == false)
                {
                    DataFile file = Db.XmlModels.Where(m => m.MappedGuid == fileGuid)
                        .Select(m => m as DataFile)
                        .FirstOrDefault();

                    if (file != null)
                    {
                        dstItem.AddData(file);
                        //since the data object has now been inserted into the submission item, it is no longer needed 
                        //to stay as a stanalone object in the XmlModel table.
                        Db.XmlModels.Remove(file);

                        //moving the physical files from the temporary upload folder to a folder identified by the GUID of the
                        //item inside the uploaded data folder
                        string dstDir = Path.Combine(ConfigHelper.DataRoot, dstItem.MappedGuid);
                        if (!Directory.Exists(dstDir))
                            Directory.CreateDirectory(dstDir);

                        string srcFile = Path.Combine(file.Path, file.LocalFileName);
                        string dstFile = Path.Combine(dstDir, file.LocalFileName);
                        File.Move(srcFile, dstFile);

                        //moving the thumbnail, if it's not a shared one
                        if (file.ThumbnailType == DataFile.eThumbnailTypes.NonShared)
                        {
                            string srcThumbnail = Path.Combine(file.Path, file.Thumbnail);
                            string dstThumbnail = Path.Combine(dstDir, file.Thumbnail);
                            File.Move(srcThumbnail, dstThumbnail);
                        }

                        //updating the file path
                        file.Path = dstDir;
                    }
                }
            }
        }

        public Item UpdateStoredItem(Item changedItem)
        {
            Item dbModel = new Item();

            if (changedItem.Id > 0)
            {
                dbModel = Db.XmlModels.Find(changedItem.Id) as Item;
            }
            else
            {
                dbModel = CreateEntity<Item>(changedItem.EntityTypeId.Value);
            }

            //updating the "value" text elements
            dbModel.UpdateValues(changedItem);

            //Processing any file attachments that have been submitted
            UpdateFiles(changedItem, dbModel);

            if (changedItem.Id > 0) //update Item
                Db.Entry(dbModel).State = EntityState.Modified;
            else
                Db.XmlModels.Add(dbModel);

            return dbModel;
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

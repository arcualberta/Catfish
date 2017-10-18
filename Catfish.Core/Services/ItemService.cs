using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Catfish.Core.Services
{
    public class ItemService: EntityService
    {
        public ItemService(CatfishDbContext db) : base(db) { }

        public string UploadRoot { get { return ConfigurationManager.AppSettings["UploadRoot"]; } }

        public string DataRoot { get { return Path.Combine(UploadRoot, "Data"); } }

        public string GetURL(string pathName)
        {
            var uri = new System.Uri(pathName);
            var converted = uri.AbsoluteUri;
            return converted;
        }

        protected void DeleteFile(DataFile file)
        {

        }

        protected string CreateGuidName(string baseName)
        {
            string filename = Guid.NewGuid().ToString();
            var idx = baseName.LastIndexOf(".");
            if (idx > 0)
                filename = filename + "." + baseName.Substring(idx + 1);
            return filename;
        }

        public List<DataFile> UploadFile(HttpContextBase context, HttpRequestBase request)
        {
            List<DataFile> files = new List<DataFile>();
            string folder = Path.Combine(UploadRoot, "Temp");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                if (!Directory.Exists(folder))
                    throw new Exception("Unable to create the upload folder " + folder);
            }

            for (int i = 0; i < request.Files.Count; i++)
            {
                DataFile file = new DataFile();

                file.FileName = request.Files[i].FileName;
                file.GuidName = CreateGuidName(file.FileName);
                file.Path = "Temp";
                file.ContentType = request.Files[i].ContentType;
                file.Thumbnail = GetThumbnail(file.ContentType);
                file.ThumbnailType = DataFile.eThumbnailTypes.Shared;
                
                request.Files[i].SaveAs(Path.Combine(UploadRoot, file.Path, file.GuidName));

                file.Serialize();
                Db.XmlModels.Add(file);
                files.Add(file);
            }
            return files;
        }

        public DataFile GetFile(int id, string name)
        {
            XmlModel model = Db.XmlModels.Find(id);
            if (model is DataFile)
                return model as DataFile;
            else if (model is Item)
                return (model as Item).Files.Where(f => f.GuidName == name).FirstOrDefault();
            else
                return null;
        }

        public Item UpdateStoredItem(Item changedItem)
        {
            Item dbModel = new Item();

            if (changedItem.Id > 0)
            {
                dbModel = Db.XmlModels.Find(changedItem.Id) as Item;
                dbModel.Deserialize();

                //updating the "value" text elements
                dbModel.UpdateValues(changedItem);

                //Deleting files in the dbModel item if those files are not in the changedItem
                if (dbModel.Files.Any()) //add checking if item contain any files
                {
                    foreach (DataFile df in dbModel.Files)
                    {
                        if (changedItem.Files.Where(f => f.GuidName == df.GuidName).Any() == false)
                        {
                            df.Data.Remove();
                            DeleteFile(df);
                        }
                    }
                }

                //Inserting new files that are in the source item that are still not in this item
                if (changedItem.Files.Any())
                {
                    foreach (DataFile df in changedItem.Files)
                    {
                        if (dbModel.Files.Where(f => f.GuidName == df.GuidName).Any() == false)
                        {
                            //since the posted model doesn't have the full property list passed back from the POST call, 
                            //we should reload it form the database and use it.
                            XmlModel tmp_file_model = Db.XmlModels.Where(m => m.Guid == df.GuidName).FirstOrDefault();
                            tmp_file_model.Deserialize();

                            dbModel.InsertChildElement("./files", tmp_file_model.Data);

                            //since we inserted the XML data of df into the XML model of the item,
                            //we no longer need to keep it in the database table. Howeber, we DO NEED to keep the files
                            //because these files are now referred by the XML File model which was inserted into the XML Item model.
                            //Deleting the File table entry corresponding to df
                            Db.XmlModels.Remove(Db.XmlModels.Find(tmp_file_model.Id));
                        }
                    }
                }

            }
            else
            {
                dbModel = CreateEntity<Item>(changedItem.EntityTypeId.Value);
                dbModel.Deserialize();
                dbModel.UpdateValues(changedItem);
            }


            //Serializing the XML model into the Content field.
            dbModel.Serialize();

            if (changedItem.Id > 0) //update Item
                Db.Entry(dbModel).State = EntityState.Modified;
            else
                Db.XmlModels.Add(dbModel);

            return dbModel;
        }


        ////public Item UpdateStoredItem(Item changedItem)
        ////{
        ////    Item dbModel = new Item();

        ////    if (changedItem.Id > 0)
        ////    {
        ////        dbModel = Db.XmlModels.Find(changedItem.Id) as Item;
        ////        dbModel.Deserialize();

        ////        //updating the "value" text elements
        ////        dbModel.UpdateValues(changedItem);

        ////        //Deleting files in the dbModel item if those files are not in the changedItem
        ////        if (dbModel.Files.Any()) //add checking if item contain any files
        ////        {
        ////            foreach (DataFile df in dbModel.Files)
        ////            {
        ////                if (changedItem.Files.Where(f => f.GuidName == df.GuidName).Any() == false)
        ////                {
        ////                    df.Data.Remove();
        ////                    DeleteFile(df);
        ////                }
        ////            }
        ////        }

        ////        //Inserting new files that are in the source item that are still not in this item
        ////        if (changedItem.Files.Any())
        ////        {
        ////            foreach (DataFile df in changedItem.Files)
        ////            {
        ////                if (dbModel.Files.Where(f => f.GuidName == df.GuidName).Any() == false)
        ////                {
        ////                    //since the posted model doesn't have the full property list passed back from the POST call, 
        ////                    //we should reload it form the database and use it.
        ////                    XmlModel tmp_file_model = Db.XmlModels.Where(m => m.Guid == df.GuidName).FirstOrDefault();
        ////                    tmp_file_model.Deserialize();

        ////                    dbModel.InsertChildElement("./files", tmp_file_model.Data);

        ////                    //since we inserted the XML data of df into the XML model of the item,
        ////                    //we no longer need to keep it in the database table. Howeber, we DO NEED to keep the files
        ////                    //because these files are now referred by the XML File model which was inserted into the XML Item model.
        ////                    //Deleting the File table entry corresponding to df
        ////                    Db.XmlModels.Remove(Db.XmlModels.Find(tmp_file_model.Id));
        ////                }
        ////            }
        ////        }

        ////    }
        ////    else{
        ////        dbModel = changedItem;
        ////    }


        ////    //Serializing the XML model into the Content field.
        ////    dbModel.Serialize();

        ////    if (changedItem.Id > 0) //update Item
        ////        Db.Entry(dbModel).State = EntityState.Modified;
        ////    else
        ////        Db.XmlModels.Add(dbModel);

        ////    return dbModel;
        ////}

        /// <summary>
        /// Adding new item to database
        /// </summary>
        /// <param name="changedItem">item that contain all modified/new value that coming from post</param>
        /// <param name="newItem">reference Item that contain all proper name/label that changedItem is lack off</param>
        /// <returns></returns>
        public Item AddItem(Item changedItem, Item newItem)
        {
            Item dbModel = Db.XmlModels.Find(changedItem.Id) as Item;
            newItem.Deserialize();

            //updating the "value" text elements
            newItem.UpdateValues(changedItem);

            //Deleting files in the dbModel item if those files are not in the changedItem
            foreach (DataFile df in newItem.Files)
            {
                if (changedItem.Files.Where(f => f.GuidName == df.GuidName).Any() == false)
                {
                    df.Data.Remove();
                    DeleteFile(df);
                }
            }

            //Inserting new files that are in the source item that are still not in this item
            foreach (DataFile df in changedItem.Files)
            {
                if (newItem.Files.Select(f => f.GuidName == df.GuidName).Any() == false)
                {
                    newItem.InsertChildElement("./files", df.Data);

                    //since we inserted the XML data of df into the XML model of the item,
                    //we no longer need to keep it in the database table. Howeber, we DO NEED to keep the files
                    //because these files are now referred by the XML File model which was inserted into the XML Item model.
                    //Deleting the File table entry corresponding to df
                    //TODO: make the following works.
                    //Db.XmlModels.Remove(Db.XmlModels.Find(df.Id));
                }
            }

            dbModel.Serialize();
            Db.Entry(dbModel).State = EntityState.Modified;

            return newItem;
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

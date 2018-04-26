using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace Catfish.Core.Services
{
    /// <summary>
    /// A Service used to perform actions on Item Entitites.
    /// </summary>
    public class ItemService: EntityService
    {
        /// <summary>
        /// Create an instance of the ItemService.
        /// </summary>
        /// <param name="db">The database context containing the needed Items.</param>
        public ItemService(CatfishDbContext db) : base(db) { }

        /// <summary>
        /// Get all items accessable by the current user.
        /// </summary>
        /// <returns>The resulting list of items.</returns>
        public IQueryable<CFItem> GetItems()
        {
            return Db.Items;
        }

        /// <summary>
        /// Get a item from the database.
        /// </summary>
        /// <param name="id">The id of the Item to obtain.</param>
        /// <returns>The requested Item from the database. A null value is returned if no item is found.</returns>
        public CFItem GetItem(int id)
        {
            return Db.Items.Where(i => i.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Get a item from the database.
        /// </summary>
        /// <param name="guid">The mapped guid of the Item to obtain.</param>
        /// <returns>The requested item from the database. A null value is returned if no item is found.</returns>
        public CFItem GetItem(string guid)
        {
            return Db.Items.Where(c => c.MappedGuid == guid).FirstOrDefault();
        }

        /// <summary>
        /// Removes an item from the database.
        /// </summary>
        /// <param name="id">The id of the item to be removed.</param>
        public void DeleteItem(int id)
        {
            CFItem model = null;
            if (id > 0)
            {
                model = GetItem(id);
                if (model != null)
                {
                    Db.Entry(model).State = EntityState.Deleted;
                }
                else
                {
                    throw new ArgumentException(string.Format("Item {0} not found.", id));
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Invalid item id {0}.", id));
            }
        }

        /// <summary>
        /// Creates a new item based on the given entity type.
        /// </summary>
        /// <param name="entityTypeId">The Id of the entity type to connect to the item.</param>
        /// <returns>The newly created item.</returns>
        public CFItem CreateItem(int entityTypeId)
        {
            return CreateEntity<CFItem>(entityTypeId);
        }

        protected void UpdateFiles(CFItem srcItem, CFItem dstItem)
        {
            UpdateFiles(srcItem.AttachmentField, dstItem);
        }

        protected void UpdateFiles(Attachment srcAttachmentField, CFItem dstItem)
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

        /// <summary>
        /// Updates a Item in the database with the the values provided. A new Item is created if one does not already exist.
        /// </summary>
        /// <param name="changedItem">The item content to be modified.</param>
        /// <returns>The modified database item.</returns>
        public CFItem UpdateStoredItem(CFItem changedItem)
        {
            CFItem dbModel = new CFItem();

            if (changedItem.Id > 0)
            {
                dbModel = Db.XmlModels.Find(changedItem.Id) as CFItem;
            }
            else
            {
                dbModel = CreateEntity<CFItem>(changedItem.EntityTypeId.Value);
            }

            //updating the "value" text elements
            dbModel.UpdateValues(changedItem);

            //Processing any file attachments that have been submitted
            UpdateFiles(changedItem, dbModel);

            if (changedItem.Id > 0)
            {//update Item
                Db.Entry(dbModel).State = EntityState.Modified;
            }
            else
            {
                dbModel.Serialize();
                Db.XmlModels.Add(dbModel);
            }

            return dbModel;
        }
    }
}

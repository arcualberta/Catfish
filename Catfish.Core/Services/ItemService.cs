using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Services
{
    public class ItemService: EntityService
    {
        public ItemService(CatfishDbContext db) : base(db) { }

        protected void DeleteFileOnDisk(string url)
        {

        }

        public Item UpdateStoredItem(Item changedItem)
        {
            Item dbModel = Db.XmlModels.Find(changedItem.Id) as Item;
            dbModel.Deserialize();

            //updating the "value" text elements
            dbModel.UpdateValues(changedItem);

            //Deleting files in the dbModel item if those files are not in the changedItem
            foreach (DataFile df in dbModel.Files)
            {
                if (changedItem.Files.Where(f => f.Url == df.Url).Any() == false)
                {
                    df.Data.Remove();
                    DeleteFileOnDisk(df.Url);
                    if (df.ThumbnailType == DataFile.eThumbnailTypes.NonShared)
                        DeleteFileOnDisk(df.Thumbnail);
                }
            }

            //Inserting new files that are in the source item that are still not in this item
            foreach (DataFile df in changedItem.Files)
            {
                if (dbModel.Files.Select(f => f.Url == df.Url).Any() == false)
                {
                    dbModel.InsertChildElement("./files", df.Data);

                    //since we inserted the XML data of df into the XML model of the item,
                    //we no longer need to keep it in the database table. Howeber, we DO NEED to keep the files
                    //because these files are now referred by the XML File model which was inserted into the XML Item model.
                    //Deleting the File table entry corresponding to df
                    Db.XmlModels.Remove(Db.XmlModels.Find(df.Id));
                }
            }

            Db.Entry(dbModel).State = EntityState.Modified;

            return dbModel;
        }
    }
}

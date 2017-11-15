using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Catfish.Core.Services
{
    public class FormService: ItemService
    {
        public FormService(CatfishDbContext db):base(db)
        {

        }

        public Item SaveFormSubmission(int collectionId, Item item)
        {
            Collection collection = collectionId > 0 ? Db.Collections.Where(c => c.Id == collectionId).FirstOrDefault() : null;
            if (collectionId > 0 && collection == null)
                throw new Exception("The target collection for form submissions does not exist.");

            ItemService srv = new ItemService(Db);
            Item updatedItem = srv.UpdateStoredItem(item);

            if (collection != null)
                updatedItem.ParentMembers.Add(collection);

            return updatedItem;
        }

        public List<DataFile> UploadTempFiles(HttpRequestBase request)
        {
            List<DataFile> files = UploadFiles(request, "temp-files");
            Db.XmlModels.AddRange(files);
            return files;
        }
    }
}

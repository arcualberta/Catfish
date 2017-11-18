using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Catfish.Core.Services
{
    public class SubmissionService: ItemService
    {
        public SubmissionService(CatfishDbContext db):base(db)
        {

        }

        public IQueryable<Form> GetSubmissionTemplates()
        {
            return Db.FormTemplates;
        }

        public Form CreateSubmissionForm(int formTemplateId)
        {
            //Obtaining the template
            Form template = Db.FormTemplates.Where(m => m.Id == formTemplateId).FirstOrDefault();

            //Creating a clone of the template and returning it. We don't want to return the template
            //itself to avoid saving user data into the template.
            Form submission = new Form() { Data = template.Data };
            return submission;
        }

        //public IQueryable<T> GetForms<T>() where T: Form
        //{
        //    IQueryable<T> ms = Db.Forms.Where(m => m is T).Select(m => m as T);
        //    return ms;
        //}
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

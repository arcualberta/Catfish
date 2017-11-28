using Catfish.Core.Models;
using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

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

        public Item SaveSubmission(Form form, string formSubmissionRef, int itemId, int entityTypeId, int formTemplateId, int collectionId)
        {
            Item submissionItem;
            if (itemId == 0)
            {
                submissionItem = CreateEntity<Item>(entityTypeId);
                Db.Items.Add(submissionItem);
            }
            else
            {
                submissionItem = Db.Items.Where(m => m.Id == itemId).FirstOrDefault();
                if (submissionItem == null)
                    throw new Exception("Specified item not found");

                Db.Entry(submissionItem).State = System.Data.Entity.EntityState.Modified;
            }

            FormSubmission storedFormSubmission = submissionItem.GetFormSubmission(formSubmissionRef);
            if(storedFormSubmission == null)
            {
                //if no stored form is available, we need to clone the template
                Form template = Db.FormTemplates.Where(m => m.Id == formTemplateId).FirstOrDefault();
                if (template == null)
                    throw new Exception("Form template does not exist.");

                storedFormSubmission = new FormSubmission();
                storedFormSubmission.ReplaceFormData(new XElement(template.Data));
                submissionItem.AddData(storedFormSubmission);
            }

            storedFormSubmission.UpdateFormData(form);

            return submissionItem;
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

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

            //Removing the audit trail from the created form since the current trail contains info
            //from the form template creation.
            XElement audit = submission.Data.Element("audit");
            if (audit != null)
                audit.Remove();

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
                submissionItem.LogChange(submissionItem.Guid, "Updated.");
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

            //If any attachments have been submitted through the form and they have not yet been included in the 
            //submission item, then include them and remove them from the main XMLModel table
            var attachmentFields = form.Fields.Where(f => f is Attachment).Select(f => f as Attachment);
            foreach(var att in attachmentFields)
                UpdateFiles(att, submissionItem);

            if(collectionId > 0)
            {
                Collection collection = Db.Collections.Where(c => c.Id == collectionId).FirstOrDefault();
                if (collection == null)
                    throw new Exception("Specified collection not found");

                collection.AppendChild(submissionItem);
            }
            return submissionItem;
        }
    }
}

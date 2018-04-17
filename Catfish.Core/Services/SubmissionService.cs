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
    /// <summary>
    /// A Service used to perform actions on AbstractForm Entities
    /// </summary>
    public class SubmissionService: ItemService
    {
        /// <summary>
        /// Create an instance of the SubmissionService.
        /// </summary>
        /// <param name="db">The database context containing the needed AbstractForms.</param>
        public SubmissionService(CatfishDbContext db):base(db)
        {

        }

        /// <summary>
        /// Get all templates for forms.
        /// </summary>
        /// <returns>All form templates.</returns>
        public IQueryable<Form> GetSubmissionTemplates()
        {
            return Db.FormTemplates;
        }

        /// <summary>
        /// Obtians a form from the database.
        /// </summary>
        /// <typeparam name="T">The type of AbstractForm to obtain.</typeparam>
        /// <param name="id">The id of the form.</param>
        /// <returns></returns>
        public T GetForm<T>(int id) where T : AbstractForm
        {
            return Db.XmlModels.Where(x => x.Id == id && x is T).FirstOrDefault() as T;
        }

        /// <summary>
        /// Saves a form into the database.
        /// </summary>
        /// <typeparam name="T">The type of AbstractForm to obtain.</typeparam>
        /// <param name="form">The form to be saved</param>
        public void SaveForm<T>(T form) where T : AbstractForm
        {
            form.Serialize();
            if(form.Id > 0)
            {
                Db.Entry(form).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                Db.XmlModels.Add(form);
            }
        }

        /// <summary>
        /// Create a form submission based on a specified form template.
        /// </summary>
        /// <param name="formTemplateId">The template to create the submission on.</param>
        /// <returns>The newly created submission.</returns>
        public Form CreateSubmissionForm(int formTemplateId)
        {
            //Obtaining the template
            Form template = Db.FormTemplates.Where(m => m.Id == formTemplateId).FirstOrDefault();

            //Creating a clone of the template and returning it. We don't want to return the template
            //itself to avoid saving user data into the template.
            Form submission = new Form() { Data = template.Data };
            submission.Id = template.Id;

            //Removing the audit trail from the created form since the current trail contains info
            //from the form template creation.
            XElement audit = submission.Data.Element("audit");
            if (audit != null)
                audit.Remove();

            return submission;
        }

        public Item SaveSubmission(Form form, string formSubmissionRef, int itemId, int entityTypeId, int formTemplateId, int collectionId, IDictionary<string,string> metadataAttributeMapping=null)
        {
            Item submissionItem;
            if (itemId == 0)
            {
                submissionItem = CreateEntity<Item>(entityTypeId);
               // submissionItem.m
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

            //MR April 10 2018
            //update metadata field's value based on the attribute mapping
            //for example if "Name mapping" mapped to the Form's Title field, grab the value of the form title and set it to Metadata Set "Name Mapping Attribute"
            EntityTypeService entityTypeService = new EntityTypeService(Db);
            EntityType entityType = entityTypeService.GetEntityTypeById(entityTypeId);
            foreach (KeyValuePair<string, string> map in metadataAttributeMapping)
            {
                //key: attributeMapping, value Form's Field's Name
                string attMapping = map.Key;
                string FieldName = map.Value;
                FormField formField = storedFormSubmission.FormData.Fields.Where(f => f.Name == FieldName).FirstOrDefault();
                var FieldValues = formField.GetValues();

                EntityTypeAttributeMapping am = entityType.AttributeMappings.Where(a => a.Name == attMapping).FirstOrDefault();
                MetadataSet ms = null;
                if(am != null)
                      ms = entityType.MetadataSets.Where(m => m.Id == am.MetadataSetId).FirstOrDefault();

                FormField field;
                if(ms != null)
                    field = ms.Fields.Where(f => f.Name == am.FieldName).FirstOrDefault();
                
                foreach (var fVal in FieldValues)
                    ms.SetFieldValue(am.FieldName, fVal.Value, fVal.LanguageCode);
            }
            //end of MR

            submissionItem.Serialize();
            return submissionItem;
        }
    }
}

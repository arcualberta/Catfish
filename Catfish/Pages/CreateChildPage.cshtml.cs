 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
using Catfish.Core.Services;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Catfish.Pages
{
    public class CreateChildPageModel : CatfishPageModelModel
    {
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly AppDbContext _db;


        [BindProperty]
        public DataItem Child { get; set; }

        [BindProperty]
        public Guid ChildTemplateId { get; set; }

        [BindProperty]
        public Guid ParentId { get; set; }

        public CreateChildPageModel(IAuthorizationService auth, IEntityTemplateService temp, ISubmissionService serv, AppDbContext db) : base(auth, serv)
        {
            _entityTemplateService = temp;
            _db = db;
        }


        public void OnGet(Guid id, Guid childTemplateId)
        {
            ParentId = id;
            ChildTemplateId = childTemplateId;

            // Load the item with the specified "id"
            Item item = _submissionService.GetSubmissionDetails(id);

            // Get the entity template which has its ID to be the above loaded item's TemplateId
            EntityTemplate template = _entityTemplateService.GetTemplate(item.TemplateId.Value);

            // Get the data item that is referred by the given childTemplateId from the template
            Child = template.GetDataItem(childTemplateId);

            //Pre-filling child form fields
            foreach(var field in Child.Fields)
            {
                var sourceReference = field.GetSourceReference(false);
                if(sourceReference != null)
                {
                    var srcDataItem = item.DataContainer.Where(di => di.Id == sourceReference.FieldContainerId).FirstOrDefault();
                    if(srcDataItem != null)
                    {
                        var srcField = srcDataItem.Fields.Where(f => f.Id == sourceReference.FieldId).FirstOrDefault();
                        if (srcField != null)
                            field.CopyValue(srcField);
                    }
                }
            }
        }


        public IActionResult OnPost()
        {
            //Creating a clone of the child entity


            // not a root entity??
            //EntityTemplate template = _entityTemplateService.GetTemplate(ChildTemplateId);
            //if (template == null)
            //    throw new Exception("Entity template with ID = " + ChildTemplateId + " not found.");

            // Get Parent Item to which Child will be added
            Item parentItem = _submissionService.GetSubmissionDetails(ParentId);
            if (parentItem == null)
                throw new Exception("Entity template with ID = " + ParentId + " not found.");

            //get template from parent
            EntityTemplate template = _entityTemplateService.GetTemplate(parentItem.TemplateId.Value);



            //When we instantantiate an instance from the template, we do not need to clone metadata sets
            //Item newItem = template.Instantiate<Item>();

            // instantantiate a version of the child and update it

            DataItem newChildItem = template.InstantiateDataItem(this.Child.Id);
            newChildItem.UpdateFieldValues(this.Child);





            //parentItem.DataContainer.Append(newChildItem); 

            //parentItem.DataContainer.Add(newChildItem);
            parentItem.DataContainer.Add(newChildItem);

            //parentItem.DataContainer.Insert(newChildItem);


            // ?newDataItem.EntityId = newItem.Id;






            //Upadet the revised  entity in the database
            //_db.Items.Add(parentItem);
            _db.Items.Update(parentItem);
            _db.SaveChanges();

            return RedirectToPage("EntityDetailsPage", new { id = parentItem.Id });
        }





    }
}
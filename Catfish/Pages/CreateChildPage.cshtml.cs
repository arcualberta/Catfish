using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.Data;
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
        }
    }
}
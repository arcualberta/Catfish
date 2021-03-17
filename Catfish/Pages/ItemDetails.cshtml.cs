using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.Extend.Fields;

namespace Catfish.Pages
{
    public class ItemDetailsModel : PageModel
    {
        private readonly AppDbContext _db;

        public Item Item { get; set; }
        public string CurrentStatus { get; set; }
        [Display(Name = "Css Class")]
        public StringField CssClass { get; set; }

        [Display(Name = "Submission Confirmation")]
        public TextField SubmissionConfirmation { get; set; }

        [Display(Name = "Authorization Failure Message")]
        public TextField AuthorizationFailureMessage { get; set; }

        public ItemDetailsModel(AppDbContext db)
        {
            _db = db;
        }
        public void OnGet(Guid id)
        {
            //TODO: retrieve the item using a service call that handles the security, meaning that it should
            //verify that the current user has access to the item

            Item = _db.Items.Where(it => it.Id == id).FirstOrDefault();

        }
    }
}

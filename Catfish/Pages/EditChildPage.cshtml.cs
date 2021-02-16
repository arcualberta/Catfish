using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Catfish.Core.Models;
using Catfish.Core.Services;

namespace Catfish.Pages
{
    public class EditChildPageModel : CatfishPageModelModel
    {
        private readonly AppDbContext _db;
        public EditChildPageModel(IAuthorizationService auth, ISubmissionService serv, AppDbContext db) : base(auth, serv)
        {
            _db = db;
        }
        public void OnGet()
        {

        }
    }
}
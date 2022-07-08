using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class EntityListPageModel : CatfishPageModelModel
    {
        protected readonly ISubmissionService _submissionService;
        public IList<Item> ItemList { get; set; }
        //public EntityListPageModel(IAuthorizationService auth, ISubmissionService serv) : base(auth, serv)
        //{
        //}
        public EntityListPageModel(ISubmissionService serv) : base(null, null)
        {
            _submissionService = serv;
        }
        public void OnGet()
        {
            ItemList = _submissionService.GetSubmissionList();
            ItemList = ItemList.OrderByDescending(it => it.Created).ToList();
        }
    }
}
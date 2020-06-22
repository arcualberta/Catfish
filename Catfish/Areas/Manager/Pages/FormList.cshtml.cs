using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha;

namespace Catfish.Areas.Manager.Pages
{
    public class FormListModel : ListPageModel
    {
        public FormListModel()
        {
            ApiRoot = "~/manager/api/forms/";
            EditPage = "~/manager/forms/edit/";
            DetailsPage = "~/manager/forms/details/";
        }
        public void OnGet()
        {
        }
    }
}

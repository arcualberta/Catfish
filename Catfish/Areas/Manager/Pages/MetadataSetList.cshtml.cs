using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Areas.Manager.Pages
{
    public class MetadataSetListModel : ListPageModel
    {
        public MetadataSetListModel()
        {
            ApiRoot = "~/manager/api/metadatasets/";
            EditPage = "~/manager/metadatasets/edit/";
            DetailsPage = "~/manager/metadatasets/details/";
        }
        public void OnGet()
        {
        }
    }
}

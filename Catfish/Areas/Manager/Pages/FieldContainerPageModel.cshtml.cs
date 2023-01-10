using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models.Contents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Areas.Manager.Pages
{
    public class FieldContainerPageModel : PageModel
    {
        public string ApiRoot { get; set; }
        public string ModelLabel { get; set; }
        public FieldContainer Model { get; set; }
        public Guid? FieldContainerId { get; set; }
    }
}

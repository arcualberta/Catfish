using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class FormIngestionViewModel
    {
        [Display(Name = "Google Spreadsheet Url")]
        public string Url { get; set; }

    }
}
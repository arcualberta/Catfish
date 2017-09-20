using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class KoBaseViewModel
    {
        public enum eStatus { Success = 1, Error, Warning }
        public int Id { get; set; }
        public string Message { get; set; }
        public eStatus Status { get; set; }

        public KoBaseViewModel()
        {
            Message = "";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models
{
    public class FieldPropertyViewModel
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string DisplayType { get; set; }
        public bool IsRequired { get; set; }
    }
}
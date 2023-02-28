using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.DTO
{
    public class AuthPatchModel
    {
        public Guid ParentId { get; set; }
        [Display(Name = "New Children to be Added")]
        public string[] NewChildren { get; set; }
        [Display(Name = "Old Children to be Deleted")]
        public string[] DeleteChildren { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Catfish.Core.Models
{
    [Table("Catfish_Groups")]
    public class Group
    {
        public enum eGroupStatus
        {
            [Display(Name = "Active")]
            Active = 1,

            [Display(Name = "Inactive")]
            Inactive,

            [Display(Name = "Deleted")]
            Deleted

        }

        public Guid Id { get; set; }
        public eGroupStatus GroupStatus { get; set; }
        public string Name { get; set; }
    }
    
}

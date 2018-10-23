using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catfish.Core.Models
{
    [NotMapped]
    public class CFAuditChangeLog
    {
        public string Target { get; set; }

        public string Description { get; set; }

       public CFAuditChangeLog(string target, string description)
        {
            Target = target;
            Description = description;
        }

    }
}

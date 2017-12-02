using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models.Forms
{
    [TypeLabel("Attachment Field")]
    public class Attachment: FormField
    {
        [NotMapped]
        public List<DataFile> Files { get; set; }

        public Attachment()
        {
            Files = new List<DataFile>();
        }
    }
}

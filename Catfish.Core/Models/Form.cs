using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models
{
    [TypeLabel("Form Template")]
    public class Form : AbstractForm
    {
        public static string TagName { get { return "form"; } }
        public override string GetTagName() { return TagName; }

        public Form()
        {
            Guid = System.Guid.NewGuid().ToString("N");
        }

        public bool CheckFileReference(string guid)
        {
            foreach(var attachmentField in Fields.Where(f => f is Attachment).Select(f => f as Attachment))
                if (attachmentField.FileGuids.Contains(guid))
                    return true;
            return false;
        }
    }
}

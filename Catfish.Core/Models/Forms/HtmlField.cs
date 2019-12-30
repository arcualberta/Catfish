using Catfish.Core.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models.Forms
{
    [CFTypeLabel("Html Field")]
    public class HtmlField : FormField
    {
        public override bool IsHidden()
        {
            return true;
        }
    }
}

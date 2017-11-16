using Catfish.Core.Models.Attributes;
using Catfish.Core.Models.Data;
using Catfish.Core.Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models
{
    [TypeLabel("Form Template")]
    public class SubmissionTemplate : Form
    {
        public override string GetTagName() { return "submission-template"; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models.Data
{
    public class FormSubmission : DataObject
    {
        public override string GetTagName() { return "form-data"; }
    }
}

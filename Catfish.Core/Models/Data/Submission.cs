using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Models.Data
{
    public class Submission : DataObject
    {
        public override string GetTagName() { return "submission"; }
    }
}

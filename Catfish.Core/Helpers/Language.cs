using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Helpers
{
    public class Language
    {
        public string Code { get; set; }
        public string Label { get; set; }
        public Language()
        {
        }

        public Language(string code, string label)
        {
            Code = code;
            Label = label;
        }
    }
}

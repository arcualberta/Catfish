using Catfish.Core.Models;
using Catfish.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Helpers
{
    public class MultilingualHelper
    {
        public static string Join(IEnumerable<TextValue> values, string separator = " / ", bool ignoreEmpty = true)
        {
            return string.Join(separator, values.Where(v => !string.IsNullOrEmpty(v.Value)).Select(v => v.Value));
        }
    }
}

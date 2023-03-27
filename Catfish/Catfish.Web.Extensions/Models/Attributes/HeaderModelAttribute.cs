using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class HeaderModelAttribute : System.Attribute
    {
        public string Name { get; set; }
        public string ViewTemplate { get; set; }
    }
}

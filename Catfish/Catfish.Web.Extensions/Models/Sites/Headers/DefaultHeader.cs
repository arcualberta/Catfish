using CatfishWebExtensions.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Sites.Headers
{
    [HeaderModel(Name = "Default Header", ViewTemplate = "Headers/_DefaultHeader")]
    public class DefaultHeader: UpperLevel
    {
        
    }
}

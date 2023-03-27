using CatfishWebExtensions.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CatfishWebExtensions.Models.Sites.Headers
{
    [HeaderModel(Name = "Bi-level Header", ViewTemplate = "Header/_BiLevelHeader")]
    public class BiLevelHeader : DefaultHeader
    {
    }
}

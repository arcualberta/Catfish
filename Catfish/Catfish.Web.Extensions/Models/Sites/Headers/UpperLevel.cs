using CatfishWebExtensions.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CatfishWebExtensions.Models.Sites.Headers
{
    [HeaderModel(Name = "Bi-level Header", ViewTemplate = "Headers/_BiLevelHeader")]
    public class UpperLevel : LowerLevel
    {
        [Field(Title = "Upper Header")]
        public HtmlField UpperHeader { get; set; }
    }
}

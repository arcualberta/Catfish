using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Blocks
{
    [BlockGroupType(Name = "Archive Preview", Category = "Content", Icon = "fas fa-archive")]
    public class ArchivePreview : Block
    {
        public StringField CssClasses { get; set; }
        public StringField PostCount { get; set; }
    }
}

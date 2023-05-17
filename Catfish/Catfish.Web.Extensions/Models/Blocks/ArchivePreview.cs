using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Blocks
{
    [BlockType(Name = "Archive Preview", Category = "Content", Icon = "fas fa-archive")]
    public class ArchivePreview : Block
    {
        [Display(Name = "Css Classes")]
        public StringField CssClasses { get; set; }

        [Display(Name = "Title")]
        public StringField Title { get; set; }

        [Display(Name = "Archive Id")]
        public StringField ArchiveId { get; set; }

        [Display(Name = "Post Count")]
        public StringField PostCount { get; set; }
    }
}

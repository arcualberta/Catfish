using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CatfishWebExtensions.Constants.Enums;

namespace CatfishWebExtensions.Models.Regions
{
    public class ArchivePageSettings
    {
        [Field(Title = "Catfish Page Layout")]
        public SelectField<eArchiveListLayout> PageLayout { get; set; }

        [Field(Title = "Catfish Post Layout")]
        public SelectField<eArchivePostLayout> PostLayout { get; set; }
    }
}

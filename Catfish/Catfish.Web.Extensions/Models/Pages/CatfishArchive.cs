using CatfishWebExtensions.Models.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Pages
{
    [PageType(Title = "Catfish archive", IsArchive = true)]
    public class CatfishArchive : StandardArchive
    {
        [Region(Title = "Archive Page Settings", Display = RegionDisplayMode.Setting)]
        public ArchivePageSettings ArchivePageSetting { get; set; }
    }
    
}

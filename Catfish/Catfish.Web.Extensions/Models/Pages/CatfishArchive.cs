using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions.Models.Pages
{
    [PageType(Title = "Catfish Archive")]
    public class CatfishArchive : StandardArchive
    {
        [Region(Title = "General", Display = RegionDisplayMode.Setting)]
        [Field(Title = "Test Field")]
        public TextField TestField { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatfishWebExtensions.Areas.Manager.Pages.styleSheets
{
    public class IndexModel : PageModel
    {
        
        public DocumentField DocumentField { get; set; }
        public void OnGet()
        {
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatfishWebExtensions.Pages
{
    public class ApiProxyTextModel : PageModel
    {
        public string ModelData { get; set; }
        public void OnGet()
        {
            var jwt = Request.HttpContext.Session.Get("JWT");

            ModelData = jwt.ToString();
        }
    }
}

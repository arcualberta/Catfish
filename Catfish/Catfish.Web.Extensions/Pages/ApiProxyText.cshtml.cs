using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CatfishWebExtensions.Pages
{
    public class ApiProxyTextModel : PageModel
    {
        public string Jwt { get; set; }
        public string Tenants { get; set; }
        public void OnGet()
        {
            var jwt = "jwt"; // Request.HttpContext.Session.Get("JWT");

            Jwt = jwt.ToString();

            Tenants = "Tenants";
        }
    }
}

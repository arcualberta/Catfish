
using CatfishExtensions.DTO;
using CatfishExtensions.Interfaces.Auth;

namespace CatfishWebExtensions.Pages
{

    public class ApiProxyTextModel : PageModel
    {
        //public readonly ITenantApiProxy _tenantProxy;
        public string Jwt { get; set; }
        public List<TenantInfo> Tenants { get; set; }
        public List<TenantRoleInfo> Roles { get; set; }

        /* public ApiProxyTextModel(ITenantApiProxy tenantProxy, string jwt, string tenants)
         {
             _tenantProxy = tenantProxy;
         }*/

        public void OnGet()
        {

            // var jwt = HttpContext.Session.GetString("JWT");

            //Jwt = jwt.ToString();

          //  Tenants = _tenantProxy.GetTenants(0, 10, Jwt).ToString();
        }
    }
}

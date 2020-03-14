using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.Manager;

namespace Catfish.Areas.Manager.Pages
{
    [Authorize(Policy = Permission.Pages)]
    public class EntityTypeListModel : PageModel
    {
    }
}

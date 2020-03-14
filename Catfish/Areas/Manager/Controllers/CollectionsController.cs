using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Catfish.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("manager/collections")]
    public class CollectionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
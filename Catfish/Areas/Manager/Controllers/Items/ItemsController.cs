using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Catfish.Areas.Manager.Items
{
    public class ItemsController : Controller
    {
       
        [Area("Manager")]
        [Route("manager/items")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
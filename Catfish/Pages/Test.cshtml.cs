using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PolymorphicModelBindingSample.ModelBinders;

namespace Catfish.Pages
{
    public class TestModel : PageModel
    {
        public List<Device> Devices { get; set; }
        public void OnGet()
        {
            Devices = new List<Device>();

            Device d = new Laptop() { CPUIndex = "4 cpus" };
            // d.Kind = 
            Devices.Add(new Laptop() { CPUIndex = "4 cpus" });
        }
    }
}
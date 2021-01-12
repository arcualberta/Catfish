using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class CustomStylesModel : PageModel
    {
        private string _customCssFile;
        [BindProperty]
        [DataType(DataType.MultilineText)]
        public string Styles { get; set; }

        public CustomStylesModel(IWebHostEnvironment env)
        {
            _customCssFile = env.ContentRootPath + "\\wwwroot\\assets\\css\\public\\custom.css";
        }

        public void OnGet()
        {
            Styles = System.IO.File.Exists(_customCssFile) ? System.IO.File.ReadAllText(_customCssFile) : "";
        }

        public void OnPost()
        {
            string currentSyles = System.IO.File.Exists(_customCssFile) ? System.IO.File.ReadAllText(_customCssFile) : "";
            var compactCurrentSyles = Regex.Replace(currentSyles, @"\s+", "");
            string compactNewStyles = Regex.Replace(Styles, @"\s+", "");

            if(currentSyles.Length > 0 && compactCurrentSyles != compactNewStyles)
            {
                string backup = _customCssFile + ".bak";
                System.IO.File.AppendAllText(backup, "==== " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ====\r\n");
                System.IO.File.AppendAllText(backup, currentSyles);
            }

            System.IO.File.WriteAllText(_customCssFile, Styles);
        }

    }
}

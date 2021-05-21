using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    public class CustomStylesModel : PageModel
    {
        private string _customCssFile;
        private const string __fileName = "custom.css";

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _db;

        [BindProperty]
        [DataType(DataType.MultilineText)]
        public string Styles { get; set; }

        public CustomStylesModel(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, AppDbContext db)
        {
            _customCssFile = env.ContentRootPath + "\\wwwroot\\assets\\css\\public\\" + __fileName;
            _httpContextAccessor = httpContextAccessor;
            _db = db;
        }

        public void OnGet()
        {
            Styles = System.IO.File.Exists(_customCssFile) ? System.IO.File.ReadAllText(_customCssFile) : "";
        }

        public void OnPost()
        {
            if (Styles == null)
                Styles = "";
            string currentSyles = System.IO.File.Exists(_customCssFile) ? System.IO.File.ReadAllText(_customCssFile) : "";
            var compactCurrentSyles = Regex.Replace(currentSyles, @"\s+", "");
            string compactNewStyles = Regex.Replace(Styles, @"\s+", "");

            if(currentSyles.Length > 0 && compactCurrentSyles != compactNewStyles)
            {
                Backup backup = new Backup()
                {
                    SourceData = currentSyles,
                    SourceId = Guid.Empty,
                    SourceType = __fileName,
                    Id = Guid.NewGuid(),
                    Timestamp = DateTime.Now,
                    UserId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
                    Username = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value

                };
                _db.Backups.Add(backup);
                _db.SaveChanges();
                ////string backup = _customCssFile + ".bak";
                ////System.IO.File.AppendAllText(backup, "==== " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ====\r\n");
                ////System.IO.File.AppendAllText(backup, currentSyles);
            }

            System.IO.File.WriteAllText(_customCssFile, Styles);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Catfish.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Pages
{
    [Authorize(Roles = "SysAdmin")]
    public class TemplateList : PageModel
    {
        public List<TemplateListEntry> ItemTemplates { get; set; } = new List<TemplateListEntry>();
        public List<TemplateListEntry> CollectionTemplates { get; set; } = new List<TemplateListEntry>();

        public readonly AppDbContext _db;
        public TemplateList(AppDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            ItemTemplates = _db.ItemTemplates
                .Select(t => new TemplateListEntry() { Id = t.Id, Name = t.TemplateName})
                .OrderBy(t => t.Name)
                .ToList();
            CollectionTemplates = _db.CollectionTemplates
                .Select(t => new TemplateListEntry() { Id = t.Id, Name = t.TemplateName })
                .OrderBy(t => t.Name)
                .ToList();
        }
    }
    public class TemplateListEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

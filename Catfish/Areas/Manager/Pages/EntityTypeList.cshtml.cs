using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using Catfish.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Piranha.Manager;
using System.Collections.Generic;
using System.Linq;

namespace Catfish.Areas.Manager.Pages
{
    [Authorize(Policy = Permission.Pages)]
    public class EntityTypeListModel : PageModel
    {
        public List<EntityTemplateListEntry> Items { get; protected set; }
        public List<EntityTemplateListEntry> Collections { get; protected set; }

        public EntityTypeService _srv { get; private set; }

        public EntityTypeListModel(EntityTypeService srv)
        {
            _srv = srv;
        }

        public void OnGet()
        {
            var templates = _srv.GetEntityTemplates();

            Items = templates.Where(t => t is ItemTemplate)
                .Select(t => new EntityTemplateListEntry(t))
                .ToList()
                .OrderBy(t => t.TypeName)
                .ToList();

            Collections = templates.Where(t => t is CollectionTemplate)
                .Select(t => new EntityTemplateListEntry(t))
                .ToList()
                .OrderBy(t => t.TypeName)
                .ToList();
        }
    }
}

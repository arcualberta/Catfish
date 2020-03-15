using Catfish.Core.Models;
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
        public List<ItemTemplate> Items { get; protected set; }
        public List<CollectionTemplate> Collections { get; protected set; }

        public EntityTypeService Srv { get; private set; }

        public EntityTypeListModel(EntityTypeService srv)
        {
            Srv = srv;
        }

        public void OnGet()
        {
            var templates = Srv.GetEntityTemplates();
            Items = templates.Where(t => t is ItemTemplate).Select(t => t as ItemTemplate).ToList();
            Collections = templates.Where(t => t is CollectionTemplate).Select(t => t as CollectionTemplate).ToList();
        }
    }
}

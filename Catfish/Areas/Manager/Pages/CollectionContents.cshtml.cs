using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Catfish.Core.Models;
using Catfish.Core.Models.Contents.ViewModels.ListEntries;
using Catfish.Core.Services;
using Catfish.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Catfish.Areas.Manager.Pages
{
    public class CollectionContentsModel : PageModel
    {
        private DbEntityService _srv;
        private IEntityTemplateService _templateService;
        //public Guid Id { get; set; }
        public IList<EntityListEntry> Children { get; set; } //list of Items that in this collection
        public CollectionContentsModel(DbEntityService srv, IEntityTemplateService templateService)
        {
            _srv = srv;
            _templateService = templateService;
        }
        public void OnGet(Guid id)
        {
            
            Children = _srv.GetEntityList<Item>(0,null,id)
                .ToList()
                .OrderBy(entry => entry.ConcatenatedName)
                .ToList();

            //MR Oct 5 2021 -- if Item created from template, the Item don't have name
            // Update Oct 6 2021 -- add property "list-entry-title" to field when creatting the template, if it's "true" this field will serve as Item Label
            int i = 0;
            foreach (EntityListEntry ent in Children)
            {
                //only item created from template has templateId
                if (string.IsNullOrEmpty(ent.ConcatenatedName) && ent.TemplateId != null)
                {
                    Item item = _srv.GetItem(ent.Id, ent.TemplateId);

                    if (item != null)
                    {
                       
                        var dataItem = item.GetRootDataItem(false);

                        List<string> labels = new List<string>();
                        
                        foreach (var field in dataItem.Fields)
                        {
                            if(field.IsListEntryTitle)
                                labels.Add((field as Catfish.Core.Models.Contents.Fields.TextField).GetValue("en"));
                        }
                        
                        Children.ElementAt(i).ItemLabel = string.Join(", ", labels);
                        
                    }
                }
                i++;
            }
        }
    }
}

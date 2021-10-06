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
            //maybe just get the Title of the Item instead?
            int i = 0;
            foreach (EntityListEntry ent in Children)
            {
                //only item created from template has templateId
                if (string.IsNullOrEmpty(ent.ConcatenatedName) && ent.TemplateId != null)
                {
                    Item item = _srv.GetItem(ent.Id, ent.TemplateId);


                    // Catfish.Core.Models.Contents.Reports.BaseReport selectedReport = item.Template.Reports.ToList().ElementAt(0); //get the 1st report
                    if (item != null)
                    {
                        //var template = _templateService.GetTemplate(item.TemplateId);
                        //var rootTemplate = template.GetRootDataItem(false);
                        //var fieldList = rootTemplate.GetValueFields();
                        //List<Guid> selectedFieldGuids = new List<Guid>();

                        //foreach(var f in fieldList)
                        //{
                        //    selectedFieldGuids.Add(f.Id);
                        //}

                        var dataItem = item.GetRootDataItem(false);

                        List<string> labels = new List<string>();
                        
                        foreach (var field in dataItem.Fields)
                        {
                            if (typeof(Catfish.Core.Models.Contents.Fields.TextField).IsAssignableFrom(field.GetType()))
                                labels.Add((field as Catfish.Core.Models.Contents.Fields.TextField).GetValue("en"));
                        }
                        // only display the fisrt 2 input text box value
                        //TODO: Need to find a btter way to handle this
                        Children.ElementAt(i).ItemLabel = labels.ElementAt(0) + ", " + labels.ElementAt(1);
                        
                    }
                }
                i++;
            }
        }
    }
}

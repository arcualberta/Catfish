using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Piranha.Extend;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Core.Models.Forms;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "ListEntitiesPanel")]
    [ExportMetadata("Name", "ListEntitiesPanel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class ListEntitiesPanel : CatfishRegion
    {
        [Display(Name = "Item Per Page")]
        public int ItemPerPage { get; set; }
        
        [Display(Name = "Include Fields")]
        public List<string> Fields { get; set; }  //contain AttributeMapping Id

        [ScriptIgnore]
        public SelectList FieldsMapping { get; set; }

        [ScriptIgnore]
        public List<CFEntityTypeAttributeMapping> Mappings { get; set; }

        [ScriptIgnore]
        public List<CFItem> Items { get; set; }
        
        [ScriptIgnore]
        public int CurrentPage { get; set; }  

        [ScriptIgnore]
        public int ItemCount { get; set; }   //total items returned

        
        public ListEntitiesPanel()
        {
            Fields = new List<string>();
            CurrentPage = 1;
            Mappings = new List<CFEntityTypeAttributeMapping>();
        }
        public override void InitManager(object model)
        {
            EntityTypeService entityTypeSrv = new EntityTypeService(db);
       
            FieldsMapping = new SelectList((entityTypeSrv.GetEntityTypeAttributeMappings()), "Id", "Name");

            if (Fields.Count > 0)
            {
                foreach (var id in Fields)
                {
                    CFEntityTypeAttributeMapping map = entityTypeService.GetEntityTypeAttributeMappingById(int.Parse(id));

                    Mappings.Add(map);
                }
            }

            base.InitManager(model);
        }

        public override object GetContent(object model)
        {
            //For testing -- go to the page that use this region and add ?entity=[entityId]
            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                string pageParam = context.Request.QueryString["page"];
                string query = context.Request.QueryString["q"];

                if (string.IsNullOrWhiteSpace(query))
                {
                    query = "*:*";
                }

                int page = string.IsNullOrWhiteSpace(null) ? 0 : int.Parse(pageParam) - 1;

                if(page < 1)
                {
                    CurrentPage = 1;
                }
                else
                {
                    CurrentPage = int.Parse(pageParam);
                }
                ItemService itemService = new ItemService(db);


                Items = itemService.GetPagedItems(query, page, ItemPerPage).ToList();

               // var mappings = entityTypeSrv.GetEntityTypeAttributeMappings().Where(a => FieldsMappingId.Contains(a.Id)).OrderBy(a => FieldsMappingId.IndexOf(a.Id));
               //grab the columnHeaders
               foreach(string id in Fields)
                {
                    CFEntityTypeAttributeMapping map= entityTypeService.GetEntityTypeAttributeMappingById(int.Parse(id));
                    Mappings.Add(map);
                }

                ItemCount = Items.Count;
            }
            return base.GetContent(model);
        }

        
    }
}
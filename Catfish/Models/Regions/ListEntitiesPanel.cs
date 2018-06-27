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
        [ScriptIgnore]
        public List<SelectListItem> ListMetadataSets { get; set; }

        [ScriptIgnore]
        public List<SelectListItem> MetadataFields { get; set; }
       
        public string SelectedMetadataSet { get; set; }
        public string selectedFilterField { get; set; }

        [Display(Name = "Min Parameter")]
        public string Min_Parameter { get; set; }
        [Display(Name = "Max Parameter")]
        public string Max_Parameter { get; set; }
        [Display(Name = "Item Per Page")]
        public int ItemPerPage { get; set; }

        public string SelectedMetadataSetX { get; set; }
        public string SelectedMetadataSetY { get; set; }

        public string SelectedMetadataSetCat { get; set; }
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
            ListMetadataSets = new List<SelectListItem>();
            MetadataFields = new List<SelectListItem>();
            Fields = new List<string>();
            CurrentPage = 1;
            Mappings = new List<CFEntityTypeAttributeMapping>();
        }
        public override void InitManager(object model)
        {
            // get db context
            CatfishDbContext db = new CatfishDbContext();
            MetadataService metadataService = new MetadataService(db);
            IQueryable<CFMetadataSet> metadataSets = metadataService.GetMetadataSets();
            foreach (CFMetadataSet m in metadataSets)
            {
                ListMetadataSets.Add(new SelectListItem { Text = m.Name, Value = m.Guid });
            }

            //set the default metadatasEt Fields to the first metadataSet in the list
            if (metadataSets.Count() > 0)
            {
                if (metadataSets.ToArray()[0].Fields.Count > 0)
                {
                    foreach (FormField f in metadataSets.ToArray()[0].Fields)
                    {
                        MetadataFields.Add(new SelectListItem { Text = f.Name, Value = f.Guid });
                    }
                }
            }

            EntityTypeService entityTypeSrv = new EntityTypeService(db);
       
            FieldsMapping = new SelectList((entityTypeSrv.GetEntityTypeAttributeMappings()), "Id", "Name");
           
            base.InitManager(model);
        }

        public override object GetContent(object model)
        {
            //For testing -- go to the page that use this region and add ?entity=[entityId]
            HttpContext context = HttpContext.Current;

            if (context != null && SelectedMetadataSet != null)
            {
                string minParam = context.Request.QueryString[Min_Parameter];
                string maxParam = context.Request.QueryString[Max_Parameter];
                string pageParam = context.Request.QueryString["page"];

               int min = minParam == null ? int.MinValue : int.Parse(minParam);
                int max = maxParam == null ? int.MaxValue : int.Parse(maxParam);
                int page = pageParam == null ? 0 : int.Parse(pageParam) - 1;

                if(page == 0)
                {
                    CurrentPage = 1;
                }
                else
                {
                    CurrentPage = int.Parse(pageParam);
                }
                CatfishDbContext db = new CatfishDbContext();
                ItemService itemService = new ItemService(db);


                Items = itemService.GetPagedItems(page, ItemPerPage+1, SelectedMetadataSet, selectedFilterField, min, max).ToList();

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
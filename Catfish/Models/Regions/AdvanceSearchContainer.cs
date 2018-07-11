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
    [ExportMetadata("InternalId", "AdvanceSearchContainer")]
    [ExportMetadata("Name", "AdvanceSearchContainer")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class AdvanceSearchContainer : CatfishRegion
    {
        //[ScriptIgnore]
        //public List<SelectListItem> ListMetadataSets { get; set; }

        //[ScriptIgnore]
        //public List<SelectListItem> MetadataFields { get; set; }

       // public List<string> SelectedMetadataSets { get; set; }
       // public List<string> SelectedFields { get; set; }

       
        [Display(Name = "Include Fields")]
        public List<string> Fields { get; set; }  //contain AttributeMapping Id

        [ScriptIgnore]
        public SelectList FieldsMapping { get; set; }

        [ScriptIgnore]
        public List<CFEntityTypeAttributeMapping> Mappings { get; set; }

        [ScriptIgnore]
        public List<CFItem> Items { get; set; }

        


        public AdvanceSearchContainer()
        {
           // ListMetadataSets = new List<SelectListItem>();
           // MetadataFields = new List<SelectListItem>();
            Fields = new List<string>();
            //SelectedMetadataSets = new List<string>();
           // SelectedFields = new List<string>();
            Mappings = new List<CFEntityTypeAttributeMapping>();
        }
        public override void InitManager(object model)
        {
            // get db context
            CatfishDbContext db = new CatfishDbContext();
            //MetadataService metadataService = new MetadataService(db);
            //IQueryable<CFMetadataSet> metadataSets = metadataService.GetMetadataSets();
            //foreach (CFMetadataSet m in metadataSets)
            //{
            //    ListMetadataSets.Add(new SelectListItem { Text = m.Name, Value = m.Guid });
            //}

            ////set the default metadatasEt Fields to the first metadataSet in the list
            //if (metadataSets.Count() > 0)
            //{
            //    if (metadataSets.ToArray()[0].Fields.Count > 0)
            //    {
            //        foreach (FormField f in metadataSets.ToArray()[0].Fields)
            //        {
            //            MetadataFields.Add(new SelectListItem { Text = f.Name, Value = f.Guid });
            //        }
            //    }
            //}

            EntityTypeService entityTypeSrv = new EntityTypeService(db);

            FieldsMapping = new SelectList((entityTypeSrv.GetEntityTypeAttributeMappings()), "Id", "Name");

            base.InitManager(model);
        }

        public override object GetContent(object model)
        {
            //For testing -- go to the page that use this region and add ?entity=[entityId]
            HttpContext context = HttpContext.Current;

            //if (context != null && SelectedMetadataSet != null)
            //{
            //    string minParam = context.Request.QueryString[Min_Parameter];
            //    string maxParam = context.Request.QueryString[Max_Parameter];
            //    string pageParam = context.Request.QueryString["page"];

            //    int min = string.IsNullOrWhiteSpace(minParam) ? int.MinValue : int.Parse(minParam);
            //    int max = string.IsNullOrWhiteSpace(null) ? int.MaxValue : int.Parse(maxParam);
            //    int page = string.IsNullOrWhiteSpace(null) ? 0 : int.Parse(pageParam) - 1;

            //    if (page == 0)
            //    {
            //        CurrentPage = 1;
            //    }
            //    else
            //    {
            //        CurrentPage = int.Parse(pageParam);
            //    }
            CatfishDbContext db = new CatfishDbContext();
            ItemService itemService = new ItemService(db);


            Items = itemService.GetPagedItems(page, ItemPerPage, SelectedMetadataSet, selectedFilterField, min, max).ToList();

            // var mappings = entityTypeSrv.GetEntityTypeAttributeMappings().Where(a => FieldsMappingId.Contains(a.Id)).OrderBy(a => FieldsMappingId.IndexOf(a.Id));
            //grab the columnHeaders
            foreach (string id in Fields)
            {
                CFEntityTypeAttributeMapping map = entityTypeService.GetEntityTypeAttributeMappingById(int.Parse(id));
                Mappings.Add(map);
            }

            //    ItemCount = Items.Count;
            //}
            return base.GetContent(model);
        }


    }
}
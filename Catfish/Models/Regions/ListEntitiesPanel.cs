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
        [Display(Name = "Sort By Field")]
        public int SortByField { get; set; }

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

        [ScriptIgnore]
        public string Query { get; set; }

        
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
    }
}
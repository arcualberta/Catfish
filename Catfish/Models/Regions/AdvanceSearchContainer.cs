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
using System.ComponentModel;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "AdvanceSearchContainer")]
    [ExportMetadata("Name", "AdvanceSearchContainer")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class AdvanceSearchContainer : CatfishRegion
    {
        
       
        [Display(Name = "Include Fields")]
        public List<string> Fields { get; set; }  //contain AttributeMapping Id
        [Display(Name = "Multiple Select")]
        [Description("If checked, will be display as chewckbox list otherwise a dropdownlist")]
        public List<bool> Multiples { get; set; } //if yes -- display it with checkboxes if false display with dropdown list on th epublic view
        [ScriptIgnore]
        public SelectList FieldsMapping { get; set; }

        [ScriptIgnore]
        public List<CFEntityTypeAttributeMapping> Mappings { get; set; }

        [ScriptIgnore]
        public List<CFItem> Items { get; set; }

        private static int mTestId = 0;
        [ScriptIgnore]
        public int TestId { get; set; }

        public AdvanceSearchContainer()
        {
           
            Fields = new List<string>();
         
            Mappings = new List<CFEntityTypeAttributeMapping>();
            Multiples = new List<bool>();
            
            TestId = ++mTestId;
        }
        public override void InitManager(object model)
        {
            // get db context
            CatfishDbContext db = new CatfishDbContext();
          
            EntityTypeService entityTypeSrv = new EntityTypeService(db);

            FieldsMapping = new SelectList((entityTypeSrv.GetEntityTypeAttributeMappings()), "Id", "Name");
            
            if (Fields.Count > 0)
            {
                if(Multiples == null)
                {
                    Multiples = new List<bool>();
                }

                foreach(var id in Fields)
                {
                    CFEntityTypeAttributeMapping map = entityTypeService.GetEntityTypeAttributeMappingById(int.Parse(id));
                    Mappings.Add(map);

                }
                if(Multiples.Count < Fields.Count)
                {
                    for (int i = Multiples.Count; i < Fields.Count; i++)
                    {
                        Multiples.Add(false);
                    }
                }
            }

            base.InitManager(model);
        }

        public override void Init(object model)
        {
            base.Init(model);
        }
        
        public override object GetContent(object model)
        {
            if (Fields.Count > 0)
            {
                //For testing -- go to the page that use this region and add ?entity=[entityId]
                HttpContext context = HttpContext.Current;


                //grab the columnHeaders
                foreach (string id in Fields)
                {
                    CFEntityTypeAttributeMapping map = entityTypeService.GetEntityTypeAttributeMappingById(int.Parse(id));

                    Mappings.Add(map);
                }

                if (Multiples.Count < Fields.Count)
                {
                    for (int i = Multiples.Count; i < Fields.Count; i++)
                    {
                        Multiples.Add(false);
                    }
                }
            }

            return base.GetContent(model);
        }


    }
}
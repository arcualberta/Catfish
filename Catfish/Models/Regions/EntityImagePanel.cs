using System;
using Catfish.Core.Models.Data;
using System.ComponentModel.Composition;
using Piranha.Extend;
using System.Web;
using Catfish.Core.Models;
using Catfish.Core.Services;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "EntityImagePanel")]
    [ExportMetadata("Name", "EntityImagePanel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class EntityImagePanel : CatfishRegion
    {
        public const string ENTITY_PARAM = "entity";

        public int EntityId { get; set; }
        public string FileGuid { get; set; } //image's guid

        [ScriptIgnore]
        public string ImageUrl { get; set; }
       // public List<CFDataFile> Files { get; set; }

        [ScriptIgnore]
        public CFEntity Entity { get; set; }

        public EntityImagePanel()
        {
           // Files = new List<CFDataFile>();
        }

        public override void InitManager(object model)
        {
            // get db context
            //CatfishDbContext db = new CatfishDbContext();
            //EntityTypeService entityTypeSrv = new EntityTypeService(db);

            //FieldsMapping = new SelectList(entityTypeSrv.GetEntityTypeAttributeMappings().GroupBy(e => e.Name).Select(e => e.FirstOrDefault()), "Name", "Name");
            //if (Fields.Count <= 0)
            //{
            //    foreach (SelectListItem f in FieldsMapping)
            //    {
            //        if (f.Text.Equals("Name Mapping") || f.Text.Equals("Description Mapping"))
            //            Fields.Add(f.Text);
            //    }
            //}
            base.InitManager(model);
        }


        public override object GetContent(object model)
        {
            //For testing -- go to the page that use this region and add ?entity=[entityId]
            HttpContext context = HttpContext.Current;

            if (context != null)
            {
                string entityId = context.Request.QueryString[EntityImagePanel.ENTITY_PARAM];

                if (!string.IsNullOrEmpty(entityId))
                {
                    CatfishDbContext db = new CatfishDbContext();
                    EntityService es = new EntityService(db);

                    //ItemService itemService = new ItemService(db);
                    

                    Entity = es.GetAnEntity(Convert.ToInt32(entityId));
                    EntityId = Entity.Id;

                    foreach(var f in ((CFItem)Entity).Files)
                    {
                        CFDataFile img = f;
                        FileGuid = f.Guid;
                       // ImageUrl = System.IO.Path.Combine(f.Path, f.Large);
                    }

                    //if (checkInItems && model is CFItem)
                    //    return (model as CFItem).Files.Where(f => f.Guid == guid).FirstOrDefault();
                }
            }

            return base.GetContent(model);
        }
    }
}
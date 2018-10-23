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

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "EntityContainer")]
    [ExportMetadata("Name", "EntityContainer")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class EntityContainer : CatfishRegion
    {
        public const string ENTITY_PARAM = "entity";
        public List<string> Fields { get; set; }

        [ScriptIgnore]
        public SelectList FieldsMapping { get; set; }

        [ScriptIgnore]
        public CFEntity Entity { get; set; }

        public EntityContainer()
        {
            CssClasses = "col-lg-12";
            CssStyles = "height:400px;";
            Fields = new List<string>();
           
        }

        public override void InitManager(object model)
        {
            // get db context
            CatfishDbContext db = new CatfishDbContext();
            EntityTypeService entityTypeSrv = new EntityTypeService(db);
           
            FieldsMapping = new SelectList(entityTypeSrv.GetEntityTypeAttributeMappings().GroupBy(e=>e.Name).Select(e=>e.FirstOrDefault()), "Name", "Name");
            if(Fields.Count <= 0)
            {
                foreach(SelectListItem f in FieldsMapping)
                {
                    if(f.Text.Equals("Name Mapping") || f.Text.Equals("Description Mapping"))
                           Fields.Add(f.Text);
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
                string entityId = context.Request.QueryString[EntityContainer.ENTITY_PARAM];
               
                if (!string.IsNullOrEmpty(entityId))
                {   
                    CatfishDbContext db = new CatfishDbContext();
                    EntityService es = new EntityService(db);

                    Entity = es.GetAnEntity(Convert.ToInt32(entityId));
                }
            }

            return base.GetContent(model);
        }
    }
}
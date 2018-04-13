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
using Piranha;
using Piranha.Models.Manager.PageModels;
using Catfish.Areas.Manager.Services;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "RelationshipContainer")]
    [ExportMetadata("Name", "RelationshipContainer")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class RelationshipContainer : CatfishRegion
    {
        public const string ENTITY_PARAM = "entity";
        [Display(Description = "The page that you would like it to link to.", Name = "Target Page")]
        public string PageId { get; set; }

        [ScriptIgnore]
        public string PageLink { get; set; }
        
        [ScriptIgnore]   
        public SelectList Pages { get; set; }

        [ScriptIgnore]
        public Entity Entity { get; set; }
        [ScriptIgnore]
        public List<Item> RelatedMembers { get; set; }


        public RelationshipContainer()
        {
            CssClasses = "col-lg-12";
            CssStyles = "height:400px;";
            RelatedMembers = new List<Item>();
        }

        public override void InitManager(object model)
        {
            var internalId = Config.SiteTree;
            var listModel = ListModel.Get(internalId);

            Pages = new SelectList(listModel.Pages, "Id", "Title", PageId);
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
                    RelatedMembers = (Entity as Item).ChildRelations.ToList();
                    
                }
            }
            if(PageId != null)
            {
                PageService pageService = new PageService();
                var page = pageService.GetAPage(PageId); 
                PageLink = page.Permalink;
            }
            return base.GetContent(model);
        }
    }
}
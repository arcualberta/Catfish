﻿using System;
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
    [ExportMetadata("InternalId", "MembershipContainer")]
    [ExportMetadata("Name", "MembershipContainer")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class MembershipContainer : CatfishRegion
    {
        public const string ENTITY_PARAM = "entity";
        [Display(Description = "The page that you would like it to link to.", Name = "Target Page")]
        public string PageId { get; set; }

        [ScriptIgnore]
        public string PageLink { get; set; }
        
        [ScriptIgnore]   
        public SelectList Pages { get; set; }

        [ScriptIgnore]
        public CFEntity Entity { get; set; }
        [ScriptIgnore]
        public List<CFAggregation> Memberships { get; set; }


        public MembershipContainer()
        {
            CssClasses = "col-lg-12";
            CssStyles = "height:400px;";
            Memberships = new List<CFAggregation>();
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
                    Memberships = (Entity as CFAggregation).ChildMembers.ToList();
                    
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
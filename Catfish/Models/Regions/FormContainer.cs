using Catfish.Core.Models;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "FormContainerExtension")]
    [ExportMetadata("Name", "Form Container")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class FormContainer: CatfishRegion
    {
        [Display(Name = "Form")]
        public int FormId { get; set; }

        public Form Form { get; set; }

        [Display(Name = "Entity Type")]
        public int EntityTypeId { get; set; }

        [Display(Name = "Collection")]
        public int CollectionId { get; set; }

        [NotMapped]
        public SelectList Forms { get; set; }

        [NotMapped]
        public SelectList EntityTypes { get; set; }

        [NotMapped]
        public SelectList Collections { get; set; }
      
        //[NotMapped]
        //public 

        public override void InitManager(object model)
        {
            // get db context
            CatfishDbContext db = new CatfishDbContext();
            // fetch all forms
            Forms = new SelectList(db.FormTemplates, "Id", "Name");
            // fetch all entities
            EntityTypes = new SelectList(db.EntityTypes, "Id", "Name");
            // fetch all collections
            Collections = new SelectList(db.Collections, "Id", "Name");

            // use these past fetches to show list on vies using SelectList ?

            base.InitManager(model);            
        }
    }
}
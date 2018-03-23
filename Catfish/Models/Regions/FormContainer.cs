using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Models.ViewModels;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Piranha;
using System.Web.Script.Serialization;

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
        
        [Display(Name = "Entity Type")]
        public int EntityTypeId { get; set; }

        [Display(Name = "Collection")]
        public int CollectionId { get; set; }

        [ScriptIgnore]
        public SelectList Forms { get; set; }

        [ScriptIgnore]
        public SelectList EntityTypes { get; set; }

        [ScriptIgnore]
        public SelectList Collections { get; set; }

        [ScriptIgnore]
        public FormViewModel FormViewModel { get; set; }

        public override void InitManager(object model)
        {
            // get db context
            CatfishDbContext db = new CatfishDbContext();
            CollectionService collectionSrv = new CollectionService(db);
            EntityTypeService entityTypeSrv = new EntityTypeService(db);
            SubmissionService formService = new SubmissionService(db);

            // fetch all forms
            Forms = new SelectList(formService.GetForms(), "Id", "Name");
            // fetch all entities
            EntityTypes = new SelectList(entityTypeSrv.GetEntityTypes(), "Id", "Name");
            // fetch all collections
            Collections = new SelectList(collectionSrv.GetCollections(), "Id", "Name");

            // use these past fetches to show list on vies using SelectList ?

            base.InitManager(model);            
        }

        public override object GetContent(object model)
        {
            if (FormId > 0)
            {
                SubmissionService subSrv = new SubmissionService(new CatfishDbContext());

                Form form = subSrv.CreateSubmissionForm(FormId);

                FormViewModel = new FormViewModel()
                {
                    Form = form,
                    ItemId = 0
                };
            }
            
            return base.GetContent(model);
        }
    }
}
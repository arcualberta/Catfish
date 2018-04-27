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
    public class FormContainer : CatfishRegion
    {
        [Display(Name = "Form")]
        public int FormId { get; set; }

        [Display(Name = "Entity Type")]
        public int EntityTypeId { get; set; }
        
       // public List<string> Fields_Mapping { get; set; }
       public List<FieldMapping> FieldMappings { get; set; }

        [Display(Name = "Collection")]
        public int CollectionId { get; set; }

        private SelectList mForms;
        [ScriptIgnore]
        public SelectList Forms
        {
            get
            {
                if (mForms == null)
                {
                    mForms = new SelectList(submissionService.GetSubmissionTemplates(), "Id", "Name");
                }
                return mForms;
            }
        }

        private SelectList mEntityTypes;
        [ScriptIgnore]
        public SelectList EntityTypes
        {
            get
            {
                if(mEntityTypes == null)
                {
                    mEntityTypes = new SelectList(entityTypeService.GetEntityTypes(), "Id", "Name");
                }

                return mEntityTypes;
            }
        }

        private SelectList mCollections;
        [ScriptIgnore]
        public SelectList Collections
        {
            get
            {
                if(mCollections == null)
                {
                    mCollections = new SelectList(collectionService.GetCollections(), "Id", "Name");
                }

                return mCollections;
            }
        }

        [ScriptIgnore]
        public FormViewModel FormViewModel { get; set; }

        private SelectList mFormFields;
        [ScriptIgnore]
        public SelectList FormFields
        {
            get
            {
                if(mFormFields == null)
                {
                    mFormFields = new SelectList(Array.Empty<string>());
                }

                return mFormFields;
            }
        }

        private SelectList mAttributeFields;
        [ScriptIgnore]
        public SelectList AttributesFields
        {
            get
            {
                if(mAttributeFields == null)
                {
                    mAttributeFields = new SelectList(Array.Empty<string>());
                }

                return mAttributeFields;
            }   
        }

        public FormContainer()
        {
            FieldMappings = new List<FieldMapping>();
           // Fields_Mapping = new List<string>();
        }

        
        public override void OnManagerSave(object model)
        {
           // Console.WriteLine("TEST WRITE LINE");
            base.OnManagerSave(model);
        }

        public override void InitManager(object model)
        {
            // get db context
            CatfishDbContext db = new CatfishDbContext();
            CollectionService collectionSrv = new CollectionService(db);
            EntityTypeService entityTypeService = new EntityTypeService(db);
            SubmissionService submissionSrv = new SubmissionService(db);
            

            // use these past fetches to show list on vies using SelectList ?
            Form form = db.FormTemplates.Where(f => f.Id == FormId).FirstOrDefault();
            if (form != null)
            {
                mFormFields = new SelectList(form.Fields, "Name", "Name");
            }

            if (EntityTypeId > 0)
            {
                mAttributeFields = new SelectList(entityTypeService.GetEntityTypeById(EntityTypeId).AttributeMappings, "Name", "Name");
            }
            
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
    
    [Serializable]
    public struct FieldMapping
    {
        public string AttributeName { get; set; }
        public string FieldName { get; set; }

    }
}
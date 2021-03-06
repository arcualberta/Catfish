﻿using Catfish.Core.Models;
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
using Catfish.Services;
using Catfish.Core.Models.Forms;

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

        [Display(Name = "Enable Reference Codes")]
        public bool EnableReferenceCodes { get; set; }

        [Display(Name = "Enforce Lists")]
        public bool EnforceLists { get; set; }

        [Display(Name = "Shuffle Blocks")]
        public bool ShuffleBlocks { get; set; }

        [Display(Name = "Shuffle Questions")]
        public bool ShuffleQuestions { get; set; }

        [Display(Name = "Question Step Options")]
        public CompositeFormField.eStepState QuestionStepOption { get; set; }

        [Display(Name = "Question-parts Step Options")]
        public CompositeFormField.eStepState QuestionPartsStepOption { get; set; }

        //Sept 13 2019
        [Display(Name ="Attach Item To User")]
        public bool AttachItemToUser { get; set; }

        // The maximum pixel length for an images longest dimension. If the size is less than or equal to 0, we will use the original size.
        [Display(Name = "Max Image Upload Length")]
        public int MaxPixelLength { get; set; }

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
            securityService.CreateAccessContext();

            // Console.WriteLine("TEST WRITE LINE");
            base.OnManagerSave(model);
        }

        public override void InitManager(object model)
        {
            securityService.CreateAccessContext();

            // get db context
            new Services.SecurityService(db).CreateAccessContext();

            CollectionService collectionSrv = new CollectionService(db);
            EntityTypeService entityTypeSrv = new EntityTypeService(db);
            SubmissionService formService = new SubmissionService(db);

            securityService.CreateAccessContext();

            Form form = db.FormTemplates.Where(f => f.Id == FormId).FirstOrDefault();
            List<SelectListItem> listFormFields = new List<SelectListItem>();
            if (form != null)
            {
                GetFormField(form.Fields, "", ref listFormFields);

                mFormFields = new SelectList(listFormFields, "Value", "Text");

            }

            if (EntityTypeId > 0)
            {
                mAttributeFields = new SelectList(entityTypeService.GetEntityTypeById(EntityTypeId).AttributeMappings, "Name", "Name");
            }
            
            base.InitManager(model);
        }

        public override object GetContent(object model)
        {
            securityService.CreateAccessContext();

            if (FormId > 0)
            {
                SubmissionService subSrv = new SubmissionService(new CatfishDbContext());

                Form form = subSrv.CreateSubmissionForm(
                    FormId,
                    EnforceLists,
                    ShuffleBlocks,
                    ShuffleQuestions,
                    QuestionStepOption, 
                    QuestionPartsStepOption);

                if (EnableReferenceCodes)
                {
                    Random rand = new Random();
                   form.ReferenceCode = DateTime.Now.ToString("yyMMdd-HHmmss-") + rand.Next(1000, 10000);
                }

                FormViewModel = new FormViewModel()
                {
                    Form = form,
                    ItemId = 0,
                    MaxImageSize = MaxPixelLength
                };
            }

            return base.GetContent(model);
        }
        public void GetFormField(IEnumerable<FormField> fd,string parentString,  ref List<SelectListItem> listFields)
        {
            
            foreach (Catfish.Core.Models.Forms.FormField f in fd)
            {
                if (typeof(Catfish.Core.Models.Forms.CompositeFormField).IsAssignableFrom(f.GetType()))
                {
                    string parent = parentString + f.Name + " > ";
                   this.GetFormField(((CompositeFormField)f).Fields,parent,ref listFields);
                }
                else
                {
                    listFields.Add(new SelectListItem() { Text = parentString + ((FormField)f).Name, Value = parentString + ((FormField)f).Name });
                  
                }
             
            }

           
        }

    }
    
    [Serializable]
    public struct FieldMapping
    {
        public string AttributeName { get; set; }
        public string FieldName { get; set; }

    }


   
}
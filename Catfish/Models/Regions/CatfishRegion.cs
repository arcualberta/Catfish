using Piranha.Areas.Manager.Views.Shared.EditorTemplates;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Piranha;
using System.Web.Script.Serialization;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Services;
using System.Text;
using System.Web.Mvc;

namespace Catfish.Models.Regions
{
    public class CatfishRegion : Piranha.Extend.IExtension
    {
        [ThreadStatic]
        private static IDictionary<string, CatfishRegion> mScripts;

        private CatfishDbContext mDb;
        [ScriptIgnore]
        public CatfishDbContext db
        {
            get
            {
                if(mDb == null)
                {
                    mDb = new CatfishDbContext();
                }

                return mDb;
            }
        }

        private SubmissionService mSubmissionService;
        [ScriptIgnore]
        public SubmissionService submissionService { get { if (mSubmissionService == null) { mSubmissionService = new SubmissionService(db);  } return mSubmissionService; } }

        private CollectionService mCollectionService;
        [ScriptIgnore]
        public CollectionService collectionService { get { if (mCollectionService == null) { mCollectionService = new CollectionService(db); } return mCollectionService; } }

        private EntityTypeService mEntityTypeService;
        [ScriptIgnore]
        public EntityTypeService entityTypeService { get { if (mEntityTypeService == null) { mEntityTypeService = new EntityTypeService(db); } return mEntityTypeService; } }

        private SecurityService mSecurityService;
        [ScriptIgnore]
        public SecurityService securityService { get { if (mSecurityService == null) { mSecurityService = new SecurityService(db); } return mSecurityService; } }

        [Display(Name = "CSS Id")]
        public string CssId { get; set; }

        [Display(Name = "CSS Classes")]
        public string CssClasses { get; set; }

        [Display(Name = "Styles")]
        [DataType(DataType.MultilineText)]
        public string CssStyles { get; set; }

        public CatfishRegion() : base()
        {
            mScripts = new Dictionary<string, CatfishRegion>();
        }

        public virtual void Ensure(DataContext db)
        {
        }

        public virtual object GetContent(object model)
        {
            return this;
        }

        public virtual void Init(object model)
        {
        }

        public virtual void InitManager(object model)
        {
        }

        public virtual void OnManagerDelete(object model)
        {
        }

        public virtual void OnManagerSave(object model)
        {
        }

        public IHtmlString RenderScript(string scriptUrl, UrlHelper url)
        {
            if (!mScripts.ContainsKey(scriptUrl))
            {
                mScripts.Add(scriptUrl, this);
                HtmlString result = new HtmlString(string.Format("<script src=\"{0}\"></script>\n", url.Content(scriptUrl)));
                return result;
            }

            return new HtmlString("");
        }
    }
}
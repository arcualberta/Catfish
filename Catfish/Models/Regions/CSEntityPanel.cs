using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Services;
using Catfish.Helpers;
using Catfish.Models.ViewModels;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Razor;
using System.Web.Script.Serialization;

namespace Catfish.Models.Regions
{
    [Export(typeof(IExtension))]
    [ExportMetadata("InternalId", "CSEntityPanel")]
    [ExportMetadata("Name", "CSharp Entity Panel")]
    [ExportMetadata("Type", ExtensionType.Region)]
    [Serializable]
    public class CSEntityPanel : CatfishRegion
    {
        private static IDictionary<string, Assembly> Assemblies = new Dictionary<string, Assembly>();

        [Display(Name = "Default Entity Id")]
        public int? DefaultEntityId { get; set; }

        [Display(Name = "Ignore Entity Property")]
        public bool IgnoreEntityProperty { get; set; }

        [Display(Name = "CSHTML")]
        [Required]
        public string CsHtml { get; set; }

        public byte[] CompiledCode { get; set; }

        public string ClassId { get; set; }

        [ScriptIgnore]
        public ICatfishCompiledView View { get; set; }

        [ScriptIgnore]
        public string Error { get; set; }

        public CSEntityPanel() : base()
        {
            CsHtml = "@model Catfish.Models.ViewModels.EntityViewModel\n\n" +
                "<p>@Model.Id</p>";

            Error = null;

            ClassId = "CSEntityPanel" + Guid.NewGuid().ToString("N"); // We remove the '-' in the GUID so the class name compiles correctly.
        }

        public override void InitManager(object model)
        {
            base.InitManager(model);
        }

        public string[] GetReferencedAssemblies()
        {
            return new string[]{
                typeof(System.Web.Mvc.IView).Assembly.Location,
                typeof(System.Web.HtmlString).Assembly.Location,
                typeof(RazorGenerator.Mvc.PrecompiledMvcView).Assembly.Location,
                typeof(System.Linq.Expressions.Expression).Assembly.Location,
                typeof(CFEntity).Assembly.Location,
                typeof(ViewHelper).Assembly.Location
            };
        }

        public override void OnManagerSave(object model)
        {
            CsHtml = CsHtml.Trim();
            string[] lines = CsHtml.Split(new[] { '\r', '\n' });

            // Create the result
            string modelType = "Catfish.Models.ViewModels.EntityViewModel";
            if (CsHtml.StartsWith("@model"))
            {
                modelType = lines.FirstOrDefault().Substring(6).Trim();
            }
            Assembly result;

            try
            {
                result = ViewHelper.CompileView(string.Join("\n", lines, 1, lines.Length - 1), ClassId, "Catfish.Models.Regions.CSEntityPanel", modelType);
            }catch(HttpCompileException ex)
            {
                throw (ex);
            }

            // Convert the code to binary
            CompiledCode = System.IO.File.ReadAllBytes(result.Location);

            base.OnManagerSave(model);
        }

        public override object GetContent(object model)
        {
            if (CompiledCode != null)
            {
                HttpContext context = HttpContext.Current;
                Assembly assembly = Assembly.Load(CompiledCode);
                View = (ICatfishCompiledView)Activator.CreateInstance(assembly.GetType("Catfish.Models.Regions.CSEntityPanel." + ClassId));

                string entityId = context.Request.QueryString[EntityContainer.ENTITY_PARAM];
                int id = 0;
                if(IgnoreEntityProperty || string.IsNullOrEmpty(entityId) || !int.TryParse(entityId, out id))
                {
                    if (DefaultEntityId != null)
                    {
                        id = DefaultEntityId.Value;
                    }
                }

                if(id > 0)
                {
                    EntityService es = new EntityService(db);
                    CFEntity entity = es.GetAnEntity(id);

                    if (entity == null)
                    {
                        Error = Catfish.Resources.Views.Shared.DisplayTemplates.CSEntityPanel.NotFound;
                    }
                    else
                    {
                        View.SetModel(new EntityViewModel(es.GetAnEntity(id), ConfigHelper.Languages.Select(l => l.TwoLetterISOLanguageName.ToLower()).ToArray())); //TODO: Get Language codes
                    }
                }
            }

            return base.GetContent(model);
        }
        
    }
}
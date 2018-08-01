using Catfish.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Data.Entity;
using System.Threading;
using System.Globalization;
using Catfish.Models.Regions;
using System.Web.Script.Serialization;
using Catfish.Core.Models;
using System.Reflection;
using System.Web.Razor;
using System.IO;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Catfish.Helpers
{
    public static class ViewHelper
    {
        public static CultureInfo GetActiveLanguage()
        {
            //var session = System.Web.HttpContext.Current.Session;
            //if (session["ActiveLanguage"] as CultureInfo == null)
            //    session["ActiveLanguage"] = ConfigHelper.Languages[0];

            return Thread.CurrentThread.CurrentUICulture;
        }

        public static void SetActiveLanguage(CultureInfo lang)
        {
            var session = System.Web.HttpContext.Current.Session;
            session["ActiveLanguage"] = lang;
        }

        public static void MultilingualMenuRenderer(Piranha.Web.UIHelper ui, System.Text.StringBuilder str, Piranha.Models.Sitemap page)
        {
            string menuItemText = null;
            try
            {
                CultureInfo culture = Thread.CurrentThread.CurrentCulture;

                var db = new Piranha.DataContext();
                var pageTitleRegion = db.Regions
                    .Include(r => r.RegionTemplate)
                    .Where(r => r.PageId == page.Id && r.RegionTemplate.InternalId == "PageTitle")
                    .FirstOrDefault();

                MultilingualText pageTitle = new JavaScriptSerializer().Deserialize<MultilingualText>(pageTitleRegion.InternalBody);
                menuItemText = pageTitle.GetContent(culture.TwoLetterISOLanguageName);
            }
            catch (Exception)
            {
            }

            if (string.IsNullOrEmpty(menuItemText))
                menuItemText = string.IsNullOrEmpty(page.NavigationTitle) ? page.Title : page.NavigationTitle;

            var url = ui.AbsoluteUrl("/home/" + page.Permalink);
            str.Append("<a href='" + url + "'>" + menuItemText + "</a>");

            //var regions = pageModel.Regions;
            //MultilingualText pageTitle = null;

        }

        public static Assembly CompileView(string viewCode, string defaultClassName, string defaultNamespace, string defaultBaseClass = "RazorGenerator.Mvc.PrecompiledMvcView")
        {
            string tempFileName = String.Format(@"{0}\{1}.dll",
                    Path.GetTempPath(),
                    "temp_compile" + DateTime.Now.Ticks.ToString("x"));

            var language = new CSharpRazorCodeLanguage();
            var host = new RazorEngineHost(language)
            {
                DefaultBaseClass = defaultBaseClass,
                DefaultClassName = defaultClassName,
                DefaultNamespace = defaultNamespace
            };

            host.NamespaceImports.Add("System");

            RazorTemplateEngine engine = new RazorTemplateEngine(host);
            GeneratorResults razorResult = engine.GenerateCode(new StringReader(viewCode));

            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add(typeof(ViewHelper).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(CFEntity).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(RazorGenerator.Mvc.PrecompiledMvcView).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(System.Web.Mvc.IView).Assembly.Location);

            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = false;
            parameters.OutputAssembly = tempFileName;
            parameters.TreatWarningsAsErrors = false;


            CompilerResults compilerResults = new CSharpCodeProvider()
                .CompileAssemblyFromDom(parameters, razorResult.GeneratedCode);

            if (compilerResults.Errors.HasErrors)
            {
                throw new HttpCompileException(compilerResults.Errors.ToString());
            }

            return compilerResults.CompiledAssembly;
        }
    }
}
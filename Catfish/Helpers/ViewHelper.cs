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
using System.Text;
using System.Web.Mvc;

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

        public static Assembly CompileView(string viewCode, string defaultClassName, string defaultNamespace, string baseClassGeneric = null, string defaultBaseClass = "Catfish.Helpers.CatfishCompiledView")
        {
            string tempFileName = String.Format(@"{0}\{1}.dll",
                    Path.GetTempPath(),
                    "temp_compile" + DateTime.Now.Ticks.ToString("x"));

            var language = new CSharpRazorCodeLanguage();
            var host = new RazorEngineHost(language)
            {
                DefaultBaseClass = defaultBaseClass + (string.IsNullOrEmpty(baseClassGeneric) ? string.Empty : '<' + baseClassGeneric + '>'),
                DefaultClassName = defaultClassName,
                DefaultNamespace = defaultNamespace
            };

            host.NamespaceImports.Add("System");
            host.NamespaceImports.Add("System.Web.Mvc.Html");

            RazorTemplateEngine engine = new RazorTemplateEngine(host);
            GeneratorResults razorResult = engine.GenerateCode(new StringReader(viewCode));

            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add(typeof(ViewHelper).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(CFEntity).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(RazorGenerator.Mvc.PrecompiledMvcView).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(System.Web.Mvc.IView).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(System.Web.HtmlString).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(System.Linq.Expressions.Expression).Assembly.Location);

#if DEBUG
            parameters.IncludeDebugInformation = true;
#endif

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
    
    public interface ICatfishCompiledView
    {
        void SetModel(object obj);
        void Execute(ViewContext viewContext);
    }

    public abstract class CatfishCompiledView<T> : ICatfishCompiledView
    {
        public StringBuilder Builder { get; private set; }
        public T Model { get; set; }
        public HtmlHelper<T> Html { get; set; }

        public CatfishCompiledView()
        {
            Builder = new StringBuilder();
        }

        public abstract void Execute();

        public virtual void WriteAttribute(string attr, Tuple<string, int> open, Tuple<string, int> close, Tuple<Tuple<string, int>, Tuple<object, int>, bool> data)
        {
            string value;
            if (data != null)
                value = data.Item2.Item1.ToString();
            else
                value = string.Empty;

            Builder.Append(open.Item1);
            Builder.Append(value);
            Builder.Append(close.Item1);
        }

        public virtual void Write(object value)
        {
            Builder.Append(value);
        }

        public virtual void WriteLiteral(object value)
        {
            Builder.Append(value);
        }

        public override string ToString()
        {
            return Builder.ToString();
        }

        public void SetModel(object obj)
        {
            if (typeof(T).IsAssignableFrom(obj.GetType()))
            {
                Model = (T)obj;
            }
        }

        public void Execute(ViewContext viewContext)
        {
            Html = new HtmlHelper<T>(viewContext, new ViewPage());
            Html.ViewData["Model"] = Model;

            Execute();
        }
    }
}
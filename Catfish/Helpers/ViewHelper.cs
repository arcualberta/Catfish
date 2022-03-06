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
                if (pageTitleRegion != null)
                {
                    MultilingualText pageTitle = new JavaScriptSerializer().Deserialize<MultilingualText>(pageTitleRegion.InternalBody);
                    menuItemText = pageTitle.GetContent(culture.TwoLetterISOLanguageName);
                }
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

        public static Assembly CompileView(string viewCode, string defaultClassName, string defaultNamespace, string baseClassGeneric = null, string defaultBaseClass = "Catfish.Helpers.CatfishCompiledView", IEnumerable<string> libraries = null)
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
            host.NamespaceImports.Add("System.Linq");
            host.NamespaceImports.Add("System.Web.Helpers");

            RazorTemplateEngine engine = new RazorTemplateEngine(host);
            GeneratorResults razorResult = engine.GenerateCode(new StringReader(viewCode.Trim()));

            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add(typeof(ViewHelper).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(CFEntity).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(RazorGenerator.Mvc.PrecompiledMvcView).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(IView).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(HtmlString).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(System.Linq.Expressions.Expression).Assembly.Location);
            parameters.ReferencedAssemblies.Add(typeof(System.Web.Helpers.Json).Assembly.Location);

            if(libraries != null)
            {
                foreach(string library in libraries)
                {
                    if (!parameters.ReferencedAssemblies.Contains(library))
                    {
                        parameters.ReferencedAssemblies.Add(library);
                    }
                }
            }

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
                StringBuilder error = new StringBuilder("Error compiling view: \n");
                foreach(var e in compilerResults.Errors)
                {
                    error.AppendLine(e.ToString());
                }
                throw new HttpCompileException(error.ToString());
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
        public HtmlHelper<T> Html { get; protected set; }
        public UrlHelper Url { get; protected set; }

        public CatfishCompiledView()
        {
            Builder = new StringBuilder();
        }

        public abstract void Execute();

        public virtual void WriteAttribute(string name, Tuple<string, int> open, Tuple<string, int> close, params object[] fragments)
        {
            Builder.Append(open.Item1);

            foreach(var fragment in fragments)
            {
                var stringFragment = fragment as Tuple<Tuple<string, int>, Tuple<string, int>, bool>;
                var objectFragment = stringFragment == null ? (Tuple<Tuple<string, int>, Tuple<object, int>, bool>)fragment : null;

                var writeString = stringFragment != null ? stringFragment.Item1.Item1 : objectFragment.Item1.Item1;
                var literal = stringFragment != null ? stringFragment.Item3 : objectFragment.Item3;
                var value = stringFragment != null ? stringFragment.Item2.Item1 : objectFragment.Item2.Item1;

                if (value == null)
                    continue;

                Builder.Append(writeString);

                if (literal)
                {
                    Builder.Append(value);
                }else if(value != null)
                {
                    Builder.Append(value.ToString());
                }
            }

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

            Url = new UrlHelper(viewContext.RequestContext);

            Execute();
        }
    }
}
using Catfish.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Catfish.Core.Plugins
{
    public class PluginControllerFactory : DefaultControllerFactory
    {
        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            Type type = base.GetControllerType(requestContext, controllerName);

            if (type == null && controllerName != null)
            {
                IEnumerable<string> namespaces = (IEnumerable<string>)requestContext.RouteData.DataTokens["namespaces"] ?? new List<string>();
                string className = string.Format("{0}{1}Controller", char.ToUpper(controllerName[0]), controllerName.Substring(1));

                // Check the plugins for the type.
                type = PluginContext.Current.GetTypes(namespaces, className).FirstOrDefault();
            }

            return type;
        }
    }

    public class PluginViewEngine : RazorViewEngine
    {
        public void RegisterPlugin(Plugin plugin)
        {
            string areaPath = plugin.GetAreaViewsPath();
            string viewPath = plugin.GetViewsPath();

            List<string> paths = new List<string>(this.AreaViewLocationFormats);
            paths.Add(areaPath + "{1}/{0}.cshtml");
            paths.Add(areaPath + "Shared/{0}.cshtml");
            this.AreaViewLocationFormats = paths.ToArray();

            paths = new List<string>(this.AreaMasterLocationFormats);
            paths.Add(areaPath + "{1}/{0}.cshtml");
            paths.Add(areaPath + "Shared/{0}.cshtml");
            this.AreaMasterLocationFormats = paths.ToArray();

            paths = new List<string>(this.AreaPartialViewLocationFormats);
            paths.Add(areaPath + "{1}/Partial/{0}.cshtml");
            paths.Add(areaPath + "Shared/Partial/{0}.cshtml");
            this.AreaPartialViewLocationFormats = paths.ToArray();

            paths = new List<string>(this.ViewLocationFormats);
            paths.Add(viewPath + "{1}/{0}.cshtml");
            paths.Add(viewPath + "Shared/{0}.cshtml");
            this.ViewLocationFormats = paths.ToArray();

            paths = new List<string>(this.MasterLocationFormats);
            paths.Add(viewPath + "{1}/{0}.cshtml");
            paths.Add(viewPath + "Shared/{0}.cshtml");
            this.MasterLocationFormats = paths.ToArray();

            paths = new List<string>(this.PartialViewLocationFormats);
            paths.Add(viewPath + "{1}/Partial/{0}.cshtml");
            paths.Add(viewPath + "Shared/Partial/{0}.cshtml");
            this.PartialViewLocationFormats = paths.ToArray();
        }
    }
}
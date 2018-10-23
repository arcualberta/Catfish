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
}
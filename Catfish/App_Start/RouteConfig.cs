using Catfish.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Catfish
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapMvcAttributeRoutes();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Api",
                url: "apix/{controller}/{action}/{id}",
                defaults: new { controller = "Stats", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Catfish.Controllers.Api" }
            ).DataTokens["UseNamespaceFallback"] = false;

            //multi lang
            routes.MapRoute(
                name: "DefaultLocalized",
                url: "{lang}/{controller}/{action}/{id}",
                constraints: new { lang = @"(\w{2})|(\w{2}-\w{2})" },   // en or en-US
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Catfish.Controllers" }
            ).DataTokens["UseNamespaceFallback"] = false;

            //original
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Catfish.Controllers" }
            ).DataTokens["UseNamespaceFallback"] = false;
        }
    }
}
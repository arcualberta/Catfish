using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Catfish
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "LanguageSwitch",
                url: "{controller}/{action}/{lang}",
                defaults: new { controller = "Language", action = "Switch" },
                namespaces: new[] { "Catfish.Controllers" }
            ).DataTokens["UseNamespaceFallback"] = false;


            routes.MapRoute(
                name: "Details",
                url: "{controller}/{id}",
                defaults: new { controller = "Entity", action = "Details"},
                namespaces: new[] { "Catfish.Controllers" }
            ).DataTokens["UseNamespaceFallback"] = false;

            routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				namespaces: new [] { "Catfish.Controllers" }
			).DataTokens["UseNamespaceFallback"] = false ;
		}
	}
}
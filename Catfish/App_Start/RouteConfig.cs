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
                name: "Api",
                url: "apix/{controller}/{action}/{id}",
                defaults: new { controller = "Stats", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Catfish.Controllers.Api" }
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
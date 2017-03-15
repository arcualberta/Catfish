using Piranha.WebPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Catfish
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Hooks.Head.Render += (ui, str, page, post) =>
            {
                // Do something
                str.Append("<script src=\"/Scripts/jquery-3.1.1.min.js\" type=\"text/javascript\" ></script>");
                str.Append("<script src=\"/Scripts/bootstrap.min.js\" type=\"text/javascript\" ></script>");
                str.Append("<link type=\"text/css\" rel=\"stylesheet\" href=\"/content/bootstrap.min.css\" />");
                str.Append("<link type=\"text/css\" rel=\"stylesheet\" href=\"/content/Catfish.css\" />");
            };

            Manager.Menu.Add(new Manager.MenuGroup()
            {
                InternalId = "CollectionsMenu",
                Name = "Collections",
                CssClass = "collections-icon"
            });

            Manager.Menu.Where(m => m.InternalId == "Content").Single().Items.Insert(0,
                new Manager.MenuItem()
                {
                    Name = "Items",
                    Controller = "item",
                    Action = "index",
                    Permission = "ADMIN_CONTENT",
                    SelectedActions = ""
                });

            Manager.Menu.Where(m => m.InternalId == "Content").Single().Items.Insert(0,
                new Manager.MenuItem()
                {
                    Name = "Collections",
                    Controller = "collection",
                    Action = "index",
                    Permission = "ADMIN_CONTENT",
                    SelectedActions = "index"
                });
        }
    }
}

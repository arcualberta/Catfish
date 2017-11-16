using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Piranha.WebPages;
using Catfish.Core.Models.Forms;
using Catfish.Areas.Manager.ModelBinders;
using Catfish.Core.Models;
using Catfish.Helpers;

namespace Catfish
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Metadata provider
            ModelMetadataProviders.Current = new Catfish.Areas.Manager.Helpers.ModelMetadataProvider();

            //Custom model binders 
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(FormField), new XmlModelBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(OptionsField), new XmlModelBinder());

            //Additional CSS and Javascripts
            Hooks.Head.Render += (ui, str, page, post) =>
            {
                // Do something
                str.Append("<script src=\"/Scripts/jquery-2.1.1.min.js\" type=\"text/javascript\" ></script>");
                str.Append("<script src=\"/Scripts/bootstrap.min.js\" type=\"text/javascript\" ></script>");
                str.Append("<link type=\"text/css\" rel=\"stylesheet\" href=\"/content/bootstrap.min.css\" />");
                str.Append("<link type=\"text/css\" rel=\"stylesheet\" href=\"/content/Custom.css\" />");
            };

            //Adding manager menu items
            AddManagerMenus();

            //Multilingual menu
            Hooks.Menu.RenderItemLink = ViewHelper.MultilingualMenuRenderer;
        }

        private void AddManagerMenus()
        {
            ///
            /// Content Menus
            ///
            var menubar = Manager.Menu.Where(m => m.InternalId == "Content").FirstOrDefault();
            var idx = 0;

            menubar.Items.Insert(idx++, new Manager.MenuItem()
            {
                Name = "Items",
                Action = "index",
                Controller = "items",
                Permission = "ADMIN_CONTENT"
            });

            menubar.Items.Insert(idx++, new Manager.MenuItem()
            {
                Name = "Collections",
                Action = "index",
                Controller = "collections",
                Permission = "ADMIN_CONTENT"
            });

            menubar.Items.Insert(idx++, new Manager.MenuItem()
            {
                Name = "Forms",
                Action = "index",
                Controller = "SubmissionTemplates",
                Permission = "ADMIN_CONTENT"
            });

            ///
            /// Settings Menus
            ///
            menubar = Manager.Menu.Where(m => m.InternalId == "Settings").FirstOrDefault();
            idx = 0;

            menubar.Items.Insert(idx++, new Manager.MenuItem()
            {
                Name = "Metadata Sets",
                Action = "index",
                Controller = "metadata",
                Permission = "ADMIN_CONTENT"
                //,SelectedActions = "productlist,productedit"
            });

            menubar.Items.Insert(idx++, new Manager.MenuItem()
            {
                Name = "Entity Types",
                Action = "index",
                Controller = "entitytypes",
                Permission = "ADMIN_CONTENT"
                //,SelectedActions = "productlist,productedit"
            });
        }
    }
}
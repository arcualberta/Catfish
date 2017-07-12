﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using Piranha.WebPages;
using Catfish.Core.Models.Metadata;
using Catfish.Areas.Manager.ModelBinders;

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
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(SimpleField), new MetadataFieldDefinitionBinder());
            System.Web.Mvc.ModelBinders.Binders.Add(typeof(OptionsField), new MetadataFieldDefinitionBinder());

            //Adding menu items
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
                Name = "Entity Types",
                Action = "index",
                Controller = "entitytypes",
                Permission = "ADMIN_CONTENT"
                //,SelectedActions = "productlist,productedit"
            });

            menubar.Items.Insert(idx++, new Manager.MenuItem()
            {
                  Name = "Metadata Sets",
                  Action = "index",
                  Controller = "metadata",
                  Permission = "ADMIN_CONTENT"
                  //,SelectedActions = "productlist,productedit"
            });
        }
    }
}
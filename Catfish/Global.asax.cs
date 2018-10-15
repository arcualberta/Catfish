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
using Catfish.Core.Services;
using Catfish.Core.ModelBinders;
using Catfish.Core.Validators;
using Catfish.Core.Helpers;

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
            // ModelBinders.Binders.Add(typeof(DateTime), new DateModelBinder());

            //Additional CSS and Javascripts
            Hooks.Head.Render += (ui, str, page, post) =>
            {
                // Do something
                str.Append("<script src=\"/Scripts/jquery-2.1.1.min.js\" type=\"text/javascript\" ></script>");
                str.Append("<script src=\"/Scripts/jquery-ui.min.js\" type=\"text/javascript\" ></script>");
                str.Append("<script src=\"/Scripts/bootstrap.min.js\" type=\"text/javascript\" ></script>");
                str.Append("<script src=\"/Scripts/catfish-global.js\" type=\"text/javascript\" ></script>");
                str.Append("<link type=\"text/css\" rel=\"stylesheet\" href=\"/content/jquery-ui.min.css\" />");
                str.Append("<link type=\"text/css\" rel=\"stylesheet\" href=\"/content/bootstrap.min.css\" />");
                str.Append("<link type=\"text/css\" rel=\"stylesheet\" href=\"/content/Custom.css\" />");
            };

            //Adding manager menu items
            AddManagerMenus();

            //Multilingual menu
            Hooks.Menu.RenderItemLink = ViewHelper.MultilingualMenuRenderer;
            //register multiple languange for the site --May 17 2018
            foreach (string lang in ConfigHelper.LanguagesCodes)
            {
                Piranha.WebPages.WebPiranha.RegisterCulture(lang, new System.Globalization.CultureInfo(lang));
            }


            // Setup Validation Attributes
            FormFieldValidationAttribute.CurrentLanguageCodes = () => new string[] { ViewHelper.GetActiveLanguage().TwoLetterISOLanguageName };

            // Check for a SolrConnection
            string solrString = System.Configuration.ConfigurationManager.AppSettings["SolrServer"];

            if (!string.IsNullOrEmpty(solrString))
            {
                SolrService.Init(solrString);
            }

            CFXmlModel.InitializeExternally = (CFXmlModel model) =>
            {
                if (HttpContext.Current.User != null) // If we are just loading a model, this may be null.
                {
                    string guid = HttpContext.Current.User.Identity.Name;
                    model.CreatedByGuid = guid;

                    // This is done to avoid a massive performance hit when loading models from the database
                    var ctx = Catfish.Contexts.UserContext.GetContextForUser(guid);

                    if (ctx != null && ctx.User != null)
                    {
                        model.CreatedByName = ctx.User.Firstname + " " + ctx.User.Surname;
                    }
                }
            };
            
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
                Controller = "FormTemplates",
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

            //Mr Jan 23 2018 adding tab to manager/system area
            var systemMenu = Manager.Menu.Where(m => m.InternalId == "System").FirstOrDefault();
            idx = 0;

            systemMenu.Items.Insert(idx++, new Manager.MenuItem {

                Name = "User List",
                Action = "index",
                Controller = "UserLists",
                Permission = "ADMIN_CONTENT"
            });

            systemMenu.Items.Insert(systemMenu.Items.Count, new Manager.MenuItem
            {

                Name = "Access Definitions",
                Action = "index",
                Controller = "AccessDefinitions",
                Permission = "ADMIN_CONTENT"
            });


        }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Piranha;
using Piranha.AspNetCore.Identity.SQLServer;
using Piranha.AttributeBuilder;
using Piranha.Manager.Editor;

using Piranha.Manager;
using System.Linq;
using Catfish.Models.Fields;
using Catfish.Models.Blocks;
using Piranha.Data.EF.SQLServer;
using Catfish.Core.Services;
using Catfish.Helper;
using System;
using Catfish.Solr;
using Catfish.Solr.Models;
using SolrNet;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace Catfish
{
    public class Startup
    {
        /// <summary>
        /// The application config.
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="configuration">The current configuration</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            // Configuration = configuration;

            // Initialize the IConfiguration of the ConfigHelper so that it can be used by 
            // elsewhere in the Catfish.Core project.
            //Catfish.Core.Helpers.ConfigHelper.Configuration = configuration;
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();
            Configuration = builder.Build();
          
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //localization need to be called before MVC() or other service that need it
            services.AddLocalization(options =>
             options.ResourcesPath = "Resources"
           );

            //-- add MVC service
            services.AddMvc();//.AddXmlSerializerFormatters(); // to user MVC model

          
            // Service setup for Piranha CMS
            services.AddPiranha(options =>
            {
                options.AddRazorRuntimeCompilation = true;
                options.UseFileStorage();
                options.UseImageSharp();
                options.UseManager();
                options.UseTinyMCE();
                options.UseMemoryCache();
               
                options.AddRazorRuntimeCompilation = true; //MR: Feb 11, 2020  -- Enabled run time compiler for razor, so don't need to recompile when update the view
            });

            /* sql server configuration based on ==> http://piranhacms.org/blog/announcing-80-for-net-core-31    */
            services.AddPiranhaEF<SQLServerDb>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("catfish")));
            services.AddPiranhaIdentityWithSeed<IdentitySQLServerDb>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("catfish")));

            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddPiranhaManagerOptions();


            

            // Add CatfishDbContext to the service collection. This will inject the database
            // configuration options and the application "Configuration" option to CatfishDbContext
            // instance through dependency injection.
            services.AddDbContext<Catfish.Core.Models.AppDbContext>();

          

            //MR: Feb 7 2020 -- from piranha core MVCWeb example
            services.AddControllersWithViews();

          
            services.AddPiranhaApplication();
            services.AddPiranhaFileStorage();
            services.AddPiranhaImageSharp();
            services.AddPiranhaManager();
            services.AddPiranhaTinyMCE();
            services.AddPiranhaApi();
            services.AddMemoryCache();
            services.AddPiranhaMemoryCache();

            // March 6 2020 -- Add Custom Permissions
            services.AddAuthorization(o => 
            { //read secure posts
                o.AddPolicy("ReadSecurePosts", policy => {
                    policy.RequireClaim("ReadSecurePosts", "ReadSecurePosts");
                });
                
            });

            //Catfish services
            services.AddScoped<EntityTypeService>();
            services.AddScoped<DbEntityService>();
            services.AddScoped<ItemService>();
            services.AddScoped<ICatfishAppConfiguration, ReadAppConfiguration>();
            services.AddScoped<IEmail, EmailService>();
            // Solr services
            services.AddSolrNet<SolrItemModel>($"http://localhost:8983/solr/Test");
            services.AddScoped<ISolrIndexService<SolrItemModel>, SolrIndexService<SolrItemModel, ISolrOperations<SolrItemModel>>>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApi api)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Initialize Piranha
            App.Init(api);

            // Configure cache level
            App.CacheLevel = Piranha.Cache.CacheLevel.Basic;

            // Build content types
            new ContentTypeBuilder(api)
                .AddAssembly(typeof(Startup).Assembly)
                .Build()
                .DeleteOrphans();

            // Configure Tiny MCE
            EditorConfig.FromFile("editorconfig.json");

            // March 9 2020 -- add Custom middleware that check for status 401 -- that's the error code return when security is applied to a page : 
            //ie: required login to view the page content
            app.Use(async (ctx, next) =>
            {
                if (next.GetType().GUID != Guid.Empty)
                {

                    await next();

                    if (ctx.Response.StatusCode == 401)
                    {
                        ctx.Response.Redirect("/login");
                    }
                }
            });


            // Middleware setup
            //use localization
           // var supportedCulture = new[]
           //{
           //     new CultureInfo("en"),
           //     new CultureInfo("rus")

           // };
           // var requestLocalizationOptios = new RequestLocalizationOptions
           // {
           //     DefaultRequestCulture = new RequestCulture("en"),
           //     //for formating like date, currency,etc
           //     SupportedCultures = supportedCulture,
           //     //UI string -- resources that we provided
           //     SupportedUICultures = supportedCulture

           // };
           // app.UseRequestLocalization(requestLocalizationOptios);

            app.UsePiranha();
            //MR Feb 7 2020 -- add classic MVC routing
            // Build content types -- copied from piranha core mvcWeb example
            var pageTypeBuilder = new Piranha.AttributeBuilder.PageTypeBuilder(api)
               // .AddType(typeof(Models.BlogArchive))
                .AddType(typeof(Models.StandardArchive))
                .AddType(typeof(Models.StandardPage))
                 .AddType(typeof(Models.StartPage))
                 .AddType(typeof(Models.MediaPage))
                .Build()
                .DeleteOrphans();
            var postTypeBuilder = new Piranha.AttributeBuilder.PostTypeBuilder(api)
               // .AddType(typeof(Models.BlogPost))
                .AddType(typeof(Models.StandardPost))
                .Build()
                .DeleteOrphans();
            //var siteTypeBuilder = new Piranha.AttributeBuilder.SiteTypeBuilder(api)
            //    .AddType(typeof(Models.StandardSite))
            //    .Build()
            //    .DeleteOrphans();

            // /Register middleware
            app.UseStaticFiles();
          
            app.UseRouting();
          
           // app.UseIntegratedPiranha();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UsePiranhaIdentity();
            app.UsePiranhaManager();
            app.UsePiranhaTinyMCE();

            app.UseEndpoints(endpoints =>
            {
               
                endpoints.MapDefaultControllerRoute();
               
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapPiranhaManager();
            });

            AddPartialViews();

            //add to manager menu item
            AddManagerMenus();

            //Register Piranha Custom Components 
            RegisterCustomFields();
            RegisterCustomBlocks();
            RegisterCustomScripts();
            RegisterCustomStyles();

            // March 6 2020 -- Add Custom Permissions
            AddCustomPermissions();

        }

        #region REGISTER CUSTOM COMPONENT
        private static void RegisterCustomFields()
        {
            Piranha.App.Fields.Register<TextAreaField>();
        }
        private static void RegisterCustomScripts()
        {
            App.Modules.Manager().Scripts.Add("~/assets/js/textarea-field.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/embed-block.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/catfish.itemlist.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/catfish.edititem.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/calendar-block.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/javascript-block.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/css-block.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/entitytypelist.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/contact-block.js");
        }
        private static void RegisterCustomBlocks()
        {
            //Register custom Block
            App.Blocks.Register<EmbedBlock>();
            App.Blocks.Register<CalendarBlock>();
            App.Blocks.Register<EmbedBlock>();  


            App.Blocks.Register<JavascriptBlock>();
            App.Blocks.Register<CssBlock>();
            App.Blocks.Register<CalendarBlock>();
            App.Blocks.Register<ContactFormBlock>();
        }
        private static void RegisterCustomStyles()
        {
            
             App.Modules.Get<Piranha.Manager.Module>()
                .Styles.Add("~/assets/css/custom.css");
                
        }
        #endregion

        private static void AddCustomPermissions()
        {
            App.Permissions["App"].Add(new Piranha.Security.PermissionItem
            {
                Title="Read Secure Posts",
                Name="ReadSecurePosts"
            });
        }

        private static void AddPartialViews()
        {
            //App.Modules.Manager().Partials.Add("Partial/_EntityTypeListAddEntityType");
        }

        private static void AddManagerMenus()
        {
            if (Piranha.Manager.Menu.Items.Where(m => m.Name == "Entities").FirstOrDefault() == null)
            {
                Piranha.Manager.Menu.Items.Insert(0, new MenuItem
                {
                    InternalId = "Entities",
                    Name = "Entities",
                    Css = "fas fa-object-group"

                });
            }

            ///
            /// Content Menus
            ///
            var menubar = Piranha.Manager.Menu.Items.Where(m => m.InternalId == "Entities").FirstOrDefault();
            var idx = 0;

            menubar.Items.Insert(idx++, new MenuItem
            {
                InternalId = "EntityTypes",
                Name = "EntityTypes",
                Route = "/manager/entitytypes/",
                Css = "fas fa-brain"
              
            });

            menubar.Items.Insert(idx++, new MenuItem
            {
                InternalId = "Collections",
                Name = "Collections", 
                Route = "/manager/collections/",
                Css = "fas fa-object-group"
               
            });

            menubar.Items.Insert(idx++, new MenuItem
            {
                InternalId = "Items",
                Name = "Items",
                Route = "/manager/items/",
                Css = "fas fa-object-ungroup"
               
            });
        }
    }
}

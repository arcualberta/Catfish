using Catfish.Areas.Manager.Access;
using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Core.Services.FormBuilder;
using Catfish.Core.Services.Solr;
using Catfish.Helper;
using Catfish.ModelBinders;
using Catfish.Models.Blocks;
using Catfish.Models.Fields;
using Catfish.Models.SiteTypes;
using Catfish.Services;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Piranha;
using Piranha.AspNetCore.Identity.SQLServer;
using Piranha.AttributeBuilder;
using Piranha.Data.EF.SQLServer;
using Piranha.Manager;
using Piranha.Manager.Editor;
using Piranha.Services;
using SolrNet;
using System;
using System.Linq;

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

            services.AddRazorPages()
                .AddPiranhaManagerOptions();


            // Add CatfishDbContext to the service collection. This will inject the database
            // configuration options and the application "Configuration" option to CatfishDbContext
            // instance through dependency injection.
            services.AddDbContext<AppDbContext>();
            services.AddDbContext<IdentitySQLServerDb>();

            //Feb 12 - 2020 : It's recommended to use AddDbContextPool() over AddDbContext() on .net Core > 2.2
            // it's better from the performance stand point
            //  services.AddDbContextPool<Catfish.Core.Models.AppDbContext>(options =>
            //                 options.UseSqlServer(Configuration.GetConnectionString("catfish")));

            //MR: Feb 7 2020 -- from piranha core MVCWeb example
            services.AddControllersWithViews();
            
            ////services.AddControllersWithViews()
            ////    .AddNewtonsoftJson(options =>
            ////    {
            ////        options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
            ////    });

            services.AddRazorPages()
                .AddPiranhaManagerOptions()
                .AddMvcOptions(options => options.ModelBinderProviders.Insert(0, new FormFieldModelBinderProvider()));

            services.AddPiranhaApplication();
            services.AddPiranhaFileStorage();
            services.AddPiranhaImageSharp();
            services.AddPiranhaManager();
            services.AddPiranhaTinyMCE();
            services.AddPiranhaApi();
            services.AddMemoryCache();
            services.AddPiranhaMemoryCache();

            //Additiona Piranha Services
            services.AddScoped<ISiteService, Piranha.Services.SiteService>();
            services.AddScoped<IPageService, Piranha.Services.PageService>();
            services.AddScoped<IParamService, ParamService>();
            services.AddScoped<IMediaService, Piranha.Services.MediaService>();
            
            //Catfish services
            services.AddScoped<EntityTypeService>();
            services.AddScoped<UserGroupService>();
            services.AddScoped<DbEntityService>();
            services.AddScoped<ItemService>();
            services.AddScoped<ICatfishAppConfiguration, ReadAppConfiguration>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISubmissionService, SubmissionService>();
            services.AddTransient<IWorkflowService, WorkflowService>();
            services.AddScoped<IEntityTemplateService, EntityTemplateService>();
            services.AddScoped<IFormService, FormService>();
            services.AddScoped<ICatfishInitializationService, CatfishInitializationService>();

            // Solr services
            var configSection = Configuration.GetSection("SolarConfiguration:solrCore");
            if (configSection != null && !string.IsNullOrEmpty(configSection.Value))
                services.AddSolrNet<SolrEntry>(configSection.Value);

            services.AddScoped<ISolrIndexService<SolrEntry>, SolrIndexService<SolrEntry, ISolrOperations<SolrEntry>>>();
            services.AddScoped<IQueryService, QueryService>();
            services.AddScoped<IPageIndexingService, PageIndexingService>();


            //Configure policy claims
            AbstractCatfishPolicy.BuildAllPolicies(services);
            AddManagerClaims(services);


            services.AddHttpContextAccessor();

            //HangFire background processing service
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("catfish")));
            services.AddHangfireServer();
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
                 .AddType(typeof(Models.StandardArchive))
                .AddType(typeof(Models.StandardPage))
                 .AddType(typeof(Models.StartPage))
                 .AddType(typeof(Models.MediaPage))
                .Build()
                .DeleteOrphans();

            var postTypeBuilder = new Piranha.AttributeBuilder.PostTypeBuilder(api)
                .AddType(typeof(Models.StandardPost))
                .Build()
                .DeleteOrphans();

            var siteTypeBuilder = new Piranha.AttributeBuilder.SiteTypeBuilder(api)
                .AddType(typeof(CatfishWebsite))
                .AddType(typeof(WorkflowPortal))
                .Build()
                .DeleteOrphans();

            AddHooks(app);

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

            //Performing Catfish system initialization
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetService<ICatfishInitializationService>().EnsureSystemRoles();
            }
            //var scope = app.ApplicationServices.CreateScope();
            //var service = scope.ServiceProvider.GetService<IWorkflowService>();
            //service.InitSiteStructureAsync(siteContent.Id, siteContent.TypeId).Wait();


            // March 6 2020 -- Add Custom Permissions
            AddCustomPermissions();
            AddWorkflowPermissions();

            // September 23 2020 -- Add Group Permissions
            AddManagerPermissions();

            //HangFire background processing service
            app.UseHangfireDashboard();

        }

        #region REGISTER CUSTOM COMPONENT
        private static void RegisterCustomFields()
        {
            Piranha.App.Fields.Register<TextAreaField>();
        }
        private static void RegisterCustomScripts()
        {
            //App.Modules.Manager().Scripts.Add("~/assets/js/textarea-field.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/embed-block.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/catfish.itemlist.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/catfish.edititem.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/calendar-block.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/javascript-block.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/css-block.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/navigation-block.js");
          //  App.Modules.Manager().Scripts.Add("~/assets/js/entitytypelist.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/contact-block.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/form.js"); 
            App.Modules.Manager().Scripts.Add("~/assets/js/submission-entry-point-list.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/free-search.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/submission-entry-point.js");
            //App.Modules.Manager().Scripts.Add("~/assets/js/submission-list.js");
            App.Modules.Manager().Scripts.Add("~/assets/dist/bundle.js");
            App.Modules.Manager().Scripts.Add("~/assets/dist/vendors.bundle.js");
        }
        private static void RegisterCustomBlocks()
        {
            //Register custom Block
            App.Blocks.Register<EmbedBlock>();
            App.Blocks.Register<CalendarBlock>();
            App.Blocks.Register<JavascriptBlock>();
            App.Blocks.Register<CssBlock>();
            App.Blocks.Register<ContactFormBlock>();
            App.Blocks.Register<NavigationBlock>();
            App.Blocks.Register<SubmissionEntryPointList>();
            App.Blocks.Register<FreeSearchBlock>();
            App.Blocks.Register<SubmissionEntryPoint>();
        }
        private static void RegisterCustomStyles()
        {
            
             App.Modules.Get<Piranha.Manager.Module>()
                .Styles.Add("~/assets/css/custom.css");

            App.Modules.Get<Piranha.Manager.Module>()
                .Styles.Add("~/assets/css/formEditPage.css");
            /*
             These create a warning in Chrome about SameSite cookie use.
             This is not an issue for you to fix, it is for QuillJS.
             Issue: https://github.com/quilljs/quill/issues/2869
             SOF info: https://stackoverflow.com/questions/58830297/a-cookie-associated-with-a-cross-site-resource-was-set-without-the-samesite-at
             */
            App.Modules.Get<Piranha.Manager.Module>()
                .Styles.Add("https://cdn.quilljs.com/1.3.4/quill.core.css");
            App.Modules.Get<Piranha.Manager.Module>()
                .Styles.Add("https://cdn.quilljs.com/1.3.4/quill.snow.css");
            App.Modules.Get<Piranha.Manager.Module>()
                .Styles.Add("https://cdn.quilljs.com/1.3.4/quill.bubble.css");

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

        private static void AddWorkflowPermissions()
        {
            App.Permissions["Workflow"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Create Submission",
                Name = "CreateSubmission",
                Category = "Group Title"
            });

        }

        /// <summary>
        /// This method defines the custom permissions for manager pages. These are added
        /// as checkboxes for each role, which we can use to select in order to grant the 
        /// respective permissions. Simultaneously, these permissions are also added to the 
        /// Piranha User Claims table. Then these claims can be tied into policies (see 
        /// AddManagerClaims() method).
        /// Reference: https://stackoverflow.com/questions/39125347/how-to-get-claim-inside-asp-net-core-razor-view
        /// </summary>
        private static void AddManagerPermissions()
        {
            App.Permissions["Manager"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Add Groups",
                Name = "GroupsAdd",
                Category = "Groups"
            });

            App.Permissions["Manager"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Edit Groups",
                Name = "GroupsEdit",
                Category = "Groups"
            });
            App.Permissions["Manager"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Save Groups",
                Name = "GroupsSave",
                Category = "Groups"
            });
            App.Permissions["Manager"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Delete Groups",
                Name = "GroupsDelete",
                Category = "Groups"
            });
            App.Permissions["Manager"].Add(new Piranha.Security.PermissionItem
            {
                Title = "List Group",
                Name = "GroupsList",
                Category = "Groups"
            });

            App.Permissions["Manager"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Reindex",
                Name = "Reindex",
                Category = "Processes"
            });

        }

        /// <summary>
        /// Defining a series of policies. Each policy includes the permissions each
        /// user need to possess in order to grant access to a claim through the policy.
        /// These policies are used in views to authorize access using claim-based authorization appriach.
        /// Reference: https://stackoverflow.com/questions/39125347/how-to-get-claim-inside-asp-net-core-razor-view
        /// </summary>
        /// <param name="services"></param>
        private static void AddManagerClaims(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CreateEntityPolicy",
                  policy => policy.RequireClaim("Create Submission"));
            });

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("GroupsAdd", x => x.RequireClaim("GroupsAdd"));
            //});

            ////services.AddAuthorization(options =>
            ////{
            ////    options.AddPolicy("GroupsList", x => x
            ////        .RequireClaim(CatfishPermission.GroupsList)
            ////        .RequireClaim(CatfishPermission.GroupsAdd)
            ////        //CatfishPermission.GroupsDelete,
            ////        //CatfishPermission.GroupsEdit,
            ////        //CatfishPermission.GroupsList,
            ////        //CatfishPermission.GroupsSave
            ////        ) ;
            ////});

            ////services.AddAuthorization(options =>
            ////{
            ////    options.AddPolicy("GroupsEdit", x => x.RequireClaim("GroupsEdit"));
            ////});
            ////services.AddAuthorization(options =>
            ////{
            ////    options.AddPolicy("GroupsSave", x => x.RequireClaim("GroupsSave"));
            ////});
            ////services.AddAuthorization(options =>
            ////{
            ////    options.AddPolicy("GroupsDelete", x => x.RequireClaim("GroupsDelete"));
            ////});


            services.AddAuthorization(o =>
            { //read secure posts
                o.AddPolicy("ReadSecurePosts", policy => {
                    policy.RequireClaim("ReadSecurePosts", "ReadSecurePosts");
                });
            });

            services.AddAuthorization(o =>
            {
                o.AddPolicy("CreateSubmission", policy => {
                    policy.RequireClaim("CreateSubmission", "CreateSubmission");
                });
            });

            services.AddAuthorization(o =>
            {
                o.AddPolicy("CreateSubmission", policy => {
                    policy.RequireClaim("CreateSubmission", "CreateSubmission");
                });
            });
        }


        private static void AddPartialViews()
        {
            //App.Modules.Manager().Partials.Add("Partial/_EntityTypeListAddEntityType");
        }

        private static void AddManagerMenus()
        {
            if (Piranha.Manager.Menu.Items.Where(m => m.Name == "Templates").FirstOrDefault() == null)
            {
                Piranha.Manager.Menu.Items.Insert(0, new MenuItem
                {
                    InternalId = "Templates",
                    Name = "Templates",
                    Css = "fas fa-clone"

                });
            }

            ///
            /// Templates Group Content Menus
            ///
            var menubar = Piranha.Manager.Menu.Items.Where(m => m.InternalId == "Templates").FirstOrDefault();
            var idx = 0;

            menubar.Items.Insert(idx++, new MenuItem
            {
                InternalId = "MetadataSets",
                Name = "Metadata Sets",
                Route = "/manager/metadatasets/",
                Css = "fas fa-table"

            });

            menubar.Items.Insert(idx++, new MenuItem
            {
                InternalId = "EntityTypes",
                Name = "Entity Types",
                Route = "/manager/entitytypes/",
                Css = "fab fa-elementor"

            });

            menubar.Items.Insert(idx++, new MenuItem
            {
                InternalId = "Forms",
                Name = "Forms",
                Route = "/manager/forms/",
                Css = "fab fa-wpforms"

            });


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
            /// Entities Group Content Menus
            ///
            menubar = Piranha.Manager.Menu.Items.Where(m => m.InternalId == "Entities").FirstOrDefault();
            idx = 0;

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


            ///
            ///  Group List Content Menus
            ///

            menubar = Piranha.Manager.Menu.Items.Where(m => m.InternalId == "System").FirstOrDefault();
            idx = 0;

            menubar.Items.Insert(idx++, new MenuItem
            {
                InternalId = "Groups",
                Name = "Groups",
                Route = "/manager/groups/",
                Policy = GroupPolicy.List,
                Css = "fas  fa-layer-group"

            });

            //Processes
            menubar.Items.Insert(menubar.Items.Count, new MenuItem
            {
                InternalId = "Processes",
                Name = "Processes",
                Route = "/manager/processes/",
                //Policy = CatfishPermission.ManageProcesses,
                Css = "fas  fa-bezier-curve"

            });
        }

        private void AddHooks(IApplicationBuilder app)
        {
            //Hook for generating pages required by a certain type of site
            App.Hooks.SiteContent.RegisterOnAfterSave((siteContent) => {
                var scope = app.ApplicationServices.CreateScope();
                var service = scope.ServiceProvider.GetService<IWorkflowService>();
                service.InitSiteStructureAsync(siteContent.Id, siteContent.TypeId).Wait();
            });

            //Hook for indexing contents of a page right after the page is saved.
            App.Hooks.Pages.RegisterOnAfterSave((page) => {
                var scope = app.ApplicationServices.CreateScope();
                var service = scope.ServiceProvider.GetService<IPageIndexingService>();
                service.IndexPage(page);
            });

            //Hook for indexing contents of a page right after the page is saved.
            App.Hooks.Posts.RegisterOnAfterSave((post) => {
                var scope = app.ApplicationServices.CreateScope();
                var service = scope.ServiceProvider.GetService<IPageIndexingService>();
                service.IndexPost(post);
            });

        }
    }
}

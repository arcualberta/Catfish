using Catfish.Areas.Applets.Authorization;
using Catfish.Areas.Applets.Models.Blocks;
using Catfish.Areas.Applets.Services;
using Catfish.Areas.Manager.Access;
using Catfish.Core.Authorization.Handlers;
using Catfish.Core.Helpers;
using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Core.Services.FormBuilder;
using Catfish.Core.Services.Solr;
using Catfish.Helper;
using Catfish.ModelBinders;
using Catfish.Models.Blocks;
using Catfish.Models.Blocks.TileGrid;
using Catfish.Models.Fields;
using Catfish.Models.SiteTypes;
using Catfish.Services;
using ElmahCore;
using ElmahCore.Mvc;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
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
using System.Collections.Generic;
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
            string sqlConnectionString = Configuration.GetConnectionString("catfish");

            //Configuring  session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            //localization need to be called before MVC() or other service that need it
            services.AddLocalization(options =>
             options.ResourcesPath = "Resources"
           );
            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue; //MR July 14 2021 -- to increase the form count limit -- default=1024
            });
            //-- add MVC service
            services.AddMvc()
                .AddRazorOptions(options => options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml"));

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
                options.UseSqlServer(sqlConnectionString));
            services.AddPiranhaIdentityWithSeed<IdentitySQLServerDb>(options =>
                options.UseSqlServer(sqlConnectionString));

            services.AddRazorPages()
                .AddPiranhaManagerOptions()
                .AddRazorOptions(options =>
                {
                        //options.AreaPageViewLocationFormats.Add("/Areas/Applets/BlockViews/{0}.cshtml");
                        //options.AreaViewLocationFormats.Add("/Areas/Applets/BlockViews/{0}.cshtml");

                        //options.PageViewLocationFormats.Add("/Areas/Applets/BlockViews/{0}.cshtml");
                        //options.ViewLocationExpanders.Add("/Areas/Applets/BlockViews/{0}.cshtml");
                        //options.ViewLocationFormats.Add("/Areas/Applets/BlockViews/{0}.cshtml");
                    });


            //services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
            //{
            //    options.ViewLocationFormats.Add("/Areas/Applets/BlockViews/{0}.cshtml");
            //});

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
                .AddMvcOptions(options => options.ModelBinderProviders.Insert(0, new FormFieldModelBinderProvider()))
                .AddRazorOptions(options => options.ViewLocationFormats.Add("/Areas/Applets/Views/Blocks/{0}.cshtml"))
                .AddRazorOptions(options => options.ViewLocationFormats.Add("/Areas/Applets/Views/PagePartials/{0}.cshtml"));

            //MR -- July 29 2021 -- add google Oauth login
            services.AddAuthentication()
            .AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection =
                    Configuration.GetSection("GoogleExternalLogin");

                options.ClientId = googleAuthNSection["Catfish2Oauth-ClientId"];
                options.ClientSecret = googleAuthNSection["Catfish2Oauth-ClientSecret"];
            });

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
            services.AddScoped<GroupService>();
            services.AddScoped<DbEntityService>();
            services.AddScoped<ItemService>();
            services.AddScoped<ICatfishAppConfiguration, ReadAppConfiguration>();
            services.AddScoped<IConfig, ReadConfiguration>();
            services.AddScoped<Catfish.Core.Services.IAuthorizationService, AuthorizationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<ISubmissionService, SubmissionService>();
            services.AddTransient<IWorkflowService, WorkflowService>();
            services.AddScoped<IEntityTemplateService, EntityTemplateService>();
            services.AddScoped<IFormService, FormService>();
            services.AddScoped<ICatfishInitializationService, CatfishInitializationService>();
            services.AddScoped<ICatfishSiteService, CatfishSiteService>();
            services.AddScoped<IJobService, JobService>();
            services.AddSingleton<IAppService, AppService>();
            services.AddSingleton<IBlockHelper, BlockHelper>();
            services.AddScoped<IAssetRegistry, AssetRegistry>();
            services.AddScoped<IItemAppletService, ItemAppletService>();
            services.AddScoped<IItemTemplateAppletService, ItemTemplateAppletService>();

            // Solr services
            var configSection = Configuration.GetSection("SolarConfiguration:solrCore");
            if (configSection != null && !string.IsNullOrEmpty(configSection.Value))
                services.AddSolrNet<SolrEntry>(configSection.Value);

            services.AddScoped<ISolrIndexService<SolrEntry>, SolrIndexService<SolrEntry, ISolrOperations<SolrEntry>>>();
            services.AddScoped<IQueryService, QueryService>();
            services.AddScoped<IPageIndexingService, PageIndexingService>();
            services.AddScoped<ISolrService, SolrService>();
            services.AddScoped<ISolrBatchService, SolrBatchService>();
            services.AddScoped<IItemAuthorizationHelper, ItemAuthorizationHelper>();

            //Configure policy claims
            CatfishSecurity.BuildAllPolicies(services);

            //Configuring authorization services
            services.AddScoped<IAuthorizationHelper, AuthorizationHelper>();
            services.AddScoped<IAuthorizationHandler, EntityTemplateAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, GroupAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ItemEditorAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, ItemAuthorizationHandler>();
            //services.AddSingleton<IAuthorizationHandler, DocumentAuthorizationCrudHandler>();

            services.AddScoped<IBackupService, BackupService>();

            // Add custom policies
            services.AddAuthorization(o =>
            {
                    // Read secured posts
                    o.AddPolicy("ReadSecurePages", policy =>
                        {
                    policy.RequireClaim("ReadSecurePages", "ReadSecurePages");
                });
            });

            services.AddHttpContextAccessor();

            //HangFire background processing service
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("catfish")));
            services.AddHangfireServer();

            //ELMAH Error Logger
            services.AddElmah<XmlFileErrorLog>(options =>
            {
                options.LogPath = "~/log";
                options.CheckPermissionAction = context => context.User.IsInRole("SysAdmin");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApi api)
        {
            var enableRemoteErrors = Configuration.GetSection("SiteConfig:RemoteErrors").Get<bool>();
            if (env.IsDevelopment() || enableRemoteErrors)
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

            app.UsePiranha();

            //MR Feb 7 2020 -- add classic MVC routing
            // Build content types -- copied from piranha core mvcWeb example
            var pageTypeBuilder = new Piranha.AttributeBuilder.PageTypeBuilder(api)
                 .AddType(typeof(Models.StandardArchive))
                .AddType(typeof(Models.StandardPage))
                .AddType(typeof(Models.ItemPage))
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

            AddHooks(app, api);

            // /Register middleware
            app.UseStaticFiles();

            app.UseRouting();

            // app.UseIntegratedPiranha();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UsePiranhaIdentity();
            app.UsePiranhaManager();
            app.UsePiranhaTinyMCE();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapDefaultControllerRoute();

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");


                endpoints.MapPiranhaManager();
            });

            AddPartialViews();

            //add to manager menu item
            AddManagerMenus();
            RegisterCustomFields();
            RegisterCustomStyles();

            //Register Piranha Custom Blocks 
            RegisterCustomBlocks(Configuration.GetSection("BlockConfig:Production").GetChildren());
            if (env.IsDevelopment())
            {
                RegisterCustomBlocks(Configuration.GetSection("BlockConfig:Development").GetChildren());
                RegisterCustomBlocks(Configuration.GetSection("BlockConfig:Experimental").GetChildren());
            }
			else
			{
                RegisterCustomBlocks(Configuration.GetSection("BlockConfig:Obsolete").GetChildren());
            }

            //Registering other custom scripts
            RegisterCustomScripts();

            //Performing Catfish system initialization
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetService<ICatfishInitializationService>().EnsureSystemRoles();
            }
            //var scope = app.ApplicationServices.CreateScope();
            //var service = scope.ServiceProvider.GetService<IWorkflowService>();
            //service.InitSiteStructureAsync(siteContent.Id, siteContent.TypeId).Wait();


            // September 23 2020 -- Add Group Permissions
            CatfishSecurity.AddPermissionEntriesToApp();

            //HangFire background processing service
            app.UseHangfireDashboard();

            //Elmah error handler
            app.UseElmah();
        }

        #region REGISTER CUSTOM COMPONENT
        private static void RegisterCustomFields()
        {
            Piranha.App.Fields.Register<TextAreaField>();
            Piranha.App.Fields.Register<ControlledKeywordsField>();
            Piranha.App.Fields.Register<ControlledCategoriesField>();
            Piranha.App.Fields.Register<CatfishSelectList<Entity>> ();
            Piranha.App.Fields.Register<ColorPicker>();

            Piranha.App.MediaTypes.Images.Add(".svg", "image/svg+xml", false);
            Piranha.App.MediaTypes.Documents.Add(".xls", "application/vnd.ms-excel", false);
            Piranha.App.MediaTypes.Documents.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", false);
            Piranha.App.MediaTypes.Documents.Add(".csv", "text/csv", false);
            Piranha.App.MediaTypes.Documents.Add(".doc", "application/msword", false);
            Piranha.App.MediaTypes.Documents.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingm", false);
        }
		private static void RegisterCustomScripts()
		{
			App.Modules.Manager().Scripts.Add("~/assets/js/controlled-keywords.js");
			App.Modules.Manager().Scripts.Add("~/assets/js/controlled-categories.js");
			App.Modules.Manager().Scripts.Add("~/assets/js/color-picker.js");

			//    //App.Modules.Manager().Scripts.Add("~/assets/js/textarea-field.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/embed-block.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/catfish.itemlist.js");

			//    //App.Modules.Manager().Scripts.Add("~/assets/js/catfish.edititem.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/calendar-block-vue.js");

			//    App.Modules.Manager().Scripts.Add("~/assets/js/advance-search-block.js");

			//    App.Modules.Manager().Scripts.Add("~/assets/js/javascript-block.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/css-block.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/navigation-block.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/extended-image-block.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/contact-block.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/form.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/submission-entry-point-list.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/free-search.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/submission-form.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/item-list.js");
			//    //App.Modules.Manager().Scripts.Add("~/assets/js/submission-list.js");
			//    //App.Modules.Manager().Scripts.Add("~/assets/dist/editFieldFormBundle.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/dist/editItemBundle.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/dist/vendorsManagerSide.js");

			//    App.Modules.Manager().Scripts.Add("~/assets/js/controlled-vocabulary-search.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/process-page.js");

			//    //App.Modules.Manager().Scripts.Add("~/assets/js/dropdownlist-field.js");

			//    App.Modules.Manager().Scripts.Add("~/assets/js/vue-list.js");
			//    //App.Modules.Manager().Scripts.Add("~/assets/js/vue-header.js");

			//    App.Modules.Manager().Scripts.Add("~/assets/js/workflow-editor.js");

			//    App.Modules.Manager().Scripts.Add("~/assets/js/vue-single-list-item.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/card-block-vue.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/news-feed-block-vue.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/tile-grid.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/keyword-search.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/carousel.js");
			//    App.Modules.Manager().Scripts.Add("~/assets/js/item-template-editor.js");
		}

        private void RegisterCustomBlocks(IEnumerable<IConfigurationSection> blockConfigList)
        {
            foreach (var blockConfig in blockConfigList)
                RegisterCustomBlock(blockConfig);
        }
        private void RegisterCustomBlock(IConfigurationSection blockConfig)
        {
            var typeStr = blockConfig.GetSection("Type").Value;
            var t = Type.GetType(typeStr);
            if (t == null)
                throw new Exception(string.Format("Could not find the \"type\" for {0}. Did you specify the fully qualified type correctly in appsettings?", typeStr));

            var inst = (Activator.CreateInstance(t) as ICatfishBlock);
            if (inst != null)
            {
                inst.RegisterBlock();

                var js = blockConfig.GetSection("Script").Value;
                if (!string.IsNullOrEmpty(js))
                    App.Modules.Manager().Scripts.Add(js);
            }
            else
                throw new Exception(string.Format("{0} does not implement the ICatfishBlock interface", typeStr));
        }

        //private void RegisterCustomBlocks()
        //{
        //    //Register custom Block
        //    App.Blocks.Register<EmbedBlock>();
        //    App.Blocks.Register<CalendarBlock>();
        //    App.Blocks.Register<JavascriptBlock>();
        //    App.Blocks.Register<CssBlock>();
        //    App.Blocks.Register<ContactFormBlock>();
        //    App.Blocks.Register<NavigationBlock>();
        //    App.Blocks.Register<SubmissionEntryPointList>();
        //    App.Blocks.Register<FreeSearchBlock>();
        //    App.Blocks.Register<SubmissionForm>();
        //    App.Blocks.Register<ItemListBlock>();
        //    App.Blocks.Register<ExtendedImageBlock>();
        //    App.Blocks.Register<ExtendedGalleryBlock>();
        //    App.Blocks.Register<ControlledVocabularySearchBlock>();
        //    App.Blocks.Register<VueList>();
        //    App.Blocks.Register<VueCarousel>();
        //    App.Blocks.Register<ExtendedColumnBlock>();
        //    App.Blocks.Register<AdvanceSearchBlock>();
        //    App.Blocks.Register<SingleListItem>();
        //    App.Blocks.Register<ListDisplayBlock>();
        //    App.Blocks.Register<CardBlock>();
        //    App.Blocks.Register<NewsFeedBlock>();
        //    App.Blocks.Register<TileGrid>();
        //    App.Blocks.Register<KeywordSearch>();
        //    App.Blocks.Register < Carousel>();
        //    App.Blocks.Register<ItemTemplateEditor>();
        //    App.Blocks.Register<ItemEditor>();
        //}
        private static void RegisterCustomStyles()
        {
            App.Modules.Get<Piranha.Manager.Module>()
                .Styles.Add("~/assets/css/entity.css");

            App.Modules.Get<Piranha.Manager.Module>()
                .Styles.Add("~/assets/css/formEditPage.css");

            App.Modules.Get<Piranha.Manager.Module>()
                .Styles.Add("~/assets/css/transitionAndAnimationManagerSide.css");

            App.Modules.Get<Piranha.Manager.Module>()
               .Styles.Add("~/assets/css/manager-styles.css");
            
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


        private static void AddPartialViews()
        {
            //App.Modules.Manager().Partials.Add("Partial/_EntityTypeListAddEntityType");
           
        }

        private static void AddManagerMenus()
        {
            int groupId = 1;

            //Entities group
            if (Piranha.Manager.Menu.Items.Where(m => m.Name == "Entities").FirstOrDefault() == null)
            {
                Piranha.Manager.Menu.Items.Insert(groupId++, new MenuItem
                {
                    InternalId = "Entities",
                    Name = "Entities",
                    Policy = AppSecurity.AccessEntities,
                    Css = "fas fa-object-group"

                });
            }
            //Templates group
            if (Piranha.Manager.Menu.Items.Where(m => m.Name == "Templates").FirstOrDefault() == null)
            {
                Piranha.Manager.Menu.Items.Insert(groupId++, new MenuItem
                {
                    InternalId = "Templates",
                    Name = "Templates",
                    Policy = AppSecurity.AccessTemplates,
                    Css = "fas fa-clone"

                });
            }


            var menubar = Piranha.Manager.Menu.Items.Where(m => m.InternalId == "Content").FirstOrDefault();
            menubar.Items.Insert(menubar.Items.Count, new MenuItem
            {
                InternalId = "CustomStyles",
                Name = "Custom Styles",
                Route = "/manager/customstyles/",
                Policy = AppSecurity.EditTheme,
                Css = "fas fa-table"
            });

            ///
            /// Templates Group Content Menus
            ///
            menubar = Piranha.Manager.Menu.Items.Where(m => m.InternalId == "Templates").FirstOrDefault();
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
                Policy = GroupSecurity.PageAccess,
                Css = "fas  fa-layer-group"

            });

            //Processes
            menubar.Items.Insert(menubar.Items.Count, new MenuItem
            {
                InternalId = "Processes",
                Name = "Processes",
                Route = "/manager/processes/",
                Policy = ProcessSecurity.PageAccess,
                Css = "fas  fa-bezier-curve"

            });
        }

        private void AddHooks(IApplicationBuilder app, IApi api)
        {
            //Hooks for:
            //  1. Generating pages required by a certain type of site
            //  2. Updating page and post settings
            //
            App.Hooks.SiteContent.RegisterOnAfterSave((siteContent) => {
                var scope = app.ApplicationServices.CreateScope();

                var siteService = scope.ServiceProvider.GetService<ICatfishSiteService>();
                siteService.InitSiteStructureAsync(siteContent.Id, siteContent.TypeId).Wait();

               // var catfishSiteService = scope.ServiceProvider.GetService<ICatfishSiteService>();
               // catfishSiteService.UpdateKeywordVocabularyAsync(siteContent).Wait();
            });

            App.Hooks.Pages.RegisterOnBeforeSave((page) => {
                var scope = app.ApplicationServices.CreateScope();

                //Updating keywords vocabulary
               // var catfishSiteService = scope.ServiceProvider.GetService<ICatfishSiteService>();
               // catfishSiteService.UpdateKeywordVocabularyAsync(page).Wait();
            });

            App.Hooks.Pages.RegisterOnAfterSave((page) =>
            {
                var scope = app.ApplicationServices.CreateScope();

                //Indexing page content
                var service = scope.ServiceProvider.GetService<IPageIndexingService>();
                service.IndexPage(page);
            });

            App.Hooks.Posts.RegisterOnBeforeSave((post) => {
                var scope = app.ApplicationServices.CreateScope();

                //Updating keywords vocabulary
                var catfishSiteService = scope.ServiceProvider.GetService<ICatfishSiteService>();
                catfishSiteService.UpdateKeywordVocabularyAsync(post).Wait();
            });

            App.Hooks.Posts.RegisterOnAfterSave((post) => {
                var scope = app.ApplicationServices.CreateScope();

                //Indexing post content
                var service = scope.ServiceProvider.GetService<IPageIndexingService>();
                service.IndexPost(post);
            });

            //Hook for initialize Block's variable
            //App.Hooks.Pages.RegisterOnLoad((page) =>
            //{
            //    var scope = app.ApplicationServices.CreateScope();
            //    var catfishSiteService = scope.ServiceProvider.GetService<ICatfishSiteService>();
            //    //initialize the Kywords region on page load
            //    catfishSiteService.UpdateKeywordVocabularyAsync(page).Wait();
            //});



        }
    }
}

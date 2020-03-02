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


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Piranha.Manager;
using System.Linq;
using Catfish.Models.Fields;
using Catfish.Models.Blocks;

namespace Catfish
{
    public class Startup
    {
        /// <summary>
        /// The application config.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="configuration">The current configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Initialize the IConfiguration of the ConfigHelper so that it can be used by 
            // elsewhere in the Catfish.Core project.
            Catfish.Core.Helpers.ConfigHelper.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //-- add MVC service
            services.AddMvc();//.AddXmlSerializerFormatters(); // to user MVC model

            // Service setup for Piranha CMS
            services.AddPiranha(options =>
            {
                
                options.UseFileStorage();
                options.UseImageSharp();
                options.UseManager();
                options.UseTinyMCE();
                options.UseMemoryCache();
                options.UseEF(db =>
                    db.UseSqlServer(Configuration.GetConnectionString("catfish")));
                options.UseIdentityWithSeed<IdentitySQLServerDb>(db =>
                    db.UseSqlServer(Configuration.GetConnectionString("catfish")));
                options.AddRazorRuntimeCompilation = true; //MR: Feb 11, 2020  -- Enabled run time compiler for razor, so don't need to recompile when update the view
            });

            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddPiranhaManagerOptions();

            services.AddPiranhaApplication();

            // Add CatfishDbContext to the service collection. This will inject the database
            // configuration options and the application "Configuration" option to CatfishDbContext
            // instance through dependency injection.
            services.AddDbContext<Catfish.Core.Models.CatfishDbContext>();

           //Feb 12 - 2020 : It's recommended to use AddDbContextPool() over AddDbContext() on .net Core > 2.2
           // it's better from the performance stand point
          //  services.AddDbContextPool<Catfish.Core.Models.CatfishDbContext>(options =>
          //                   options.UseSqlServer(Configuration.GetConnectionString("catfish")));
            //MR: Feb 7 2020 -- from piranha core MVCWeb example
            services.AddControllersWithViews();
            services.AddRazorPages()
                .AddPiranhaManagerOptions();

            services.AddPiranhaApplication();
            services.AddMemoryCache();
            services.AddPiranhaMemoryCache();

            

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

            // Middleware setup
            app.UsePiranha(options => {
                options.UseManager();
                options.UseTinyMCE();
                options.UseIdentity();
                
                
            });

            //MR Feb 7 2020 -- add classic MVC routing
            // Build content types -- copied from piranha core mvcWeb example
            var pageTypeBuilder = new Piranha.AttributeBuilder.PageTypeBuilder(api)
                .AddType(typeof(Models.BlogArchive))
                .AddType(typeof(Models.StandardPage))
                 .AddType(typeof(Models.StartPage))
                 .AddType(typeof(Models.MainPage))
                .Build()
                .DeleteOrphans();
            var postTypeBuilder = new Piranha.AttributeBuilder.PostTypeBuilder(api)
                .AddType(typeof(Models.BlogPost))
                .Build()
                .DeleteOrphans();
            //var siteTypeBuilder = new Piranha.AttributeBuilder.SiteTypeBuilder(api)
            //    .AddType(typeof(Models.StandardSite))
            //    .Build()
            //    .DeleteOrphans();

            // /Register middleware
            app.UseStaticFiles();
          
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UsePiranhaIdentity();
            app.UsePiranhaManager();
            app.UsePiranhaTinyMCE();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "areaRoute",
                //    pattern: "{{area:exists}/{manager:exists}/{controller}/{action=Index}/{id?}"
                //    );
                endpoints.MapDefaultControllerRoute();
                //endpoints.MapAreaControllerRoute(
                //    name : "areaRoute",
                //    areaName: "Areas",
                //    pattern: "{area:exists}/{Manager:exists}/{controller=Home}/{Action=Index}/{id?}"
                //    );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapPiranhaManager();
            });


          
            //add to manager menu item
            AddManagerMenus();

            //Register Piranha Custom Components 
            RegisterCustomFields();
            RegisterCustomBlocks();
            RegisterCustomScripts();
           // RegisterCustomStyles();

        }
        private void RegisterCustomFields()
        {
            Piranha.App.Fields.Register<TextAreaField>();
        }
        private void RegisterCustomScripts()
        {
            App.Modules.Manager().Scripts.Add("~/assets/js/textarea-field.js");
            App.Modules.Manager().Scripts.Add("~/assets/js/embed-block.js");
        }
        private void RegisterCustomBlocks()
        {
            //Register custom Block
            App.Blocks.Register<EmbedBlock>();
            App.Blocks.Register<AuthorBlock>();
        }
        private void RegisterCustomStyles()
        {
            App.Modules.Get<Piranha.Manager.Module>()
                .Styles.Add("~/assets/scss/style.scss");

        }
        private void RegisterPartialViews()
        {
            //Adding Partial View to Layout of the manager interface
            //All custom partials are rendered at the end of the body tag after the built-in modals have been added.
            //    App.Modules.Get<Piranha.Manager.Module>()
            //.Partials.Add("Partial/_MyModal");

        }
        private void AddManagerMenus()
        {
            ///
            /// Content Menus
            ///
            var menubar = Piranha.Manager.Menu.Items.Where(m => m.InternalId == "Content").FirstOrDefault();
            var idx = 0;

            menubar.Items.Insert(idx++, new MenuItem
            {
                InternalId = "MyItem",
                Name = "My  Item",
               // Params="{Controller=home}/{Action=index}/{id?}",
                Route = "/manager/items/",
                Css = "fas fa-brain",
                //Policy = "MyCustomPolicy",
                // Action = ""

            });

            
        }
    }
}

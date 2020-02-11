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
            services.AddMvc().AddMvcOptions(o=>o.EnableEndpointRouting=false); // to user MVC model

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


            //MVC style of routing -- NOT WORKING
            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "areaRoute",
                //    template: "{area:exists}/{controller}/{action}/{id?}",
                //    defaults: new { controller = "Home", action = "Index" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=cms}/{action=start}/{id?}");
            });

            //add to manager menu item
            AddManagerMenus();
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

            //menubar.Items.Insert(idx++, new Manager.MenuItem()
            //{
            //    Name = "Collections",
            //    Action = "index",
            //    Controller = "collections",
            //    Permission = "ADMIN_CONTENT"
            //});

            //menubar.Items.Insert(idx++, new Manager.MenuItem()
            //{
            //    Name = "Forms",
            //    Action = "index",
            //    Controller = "FormTemplates",
            //    Permission = "ADMIN_CONTENT"
            //});

            ///
            /// Settings Menus
            ///
            //menubar = Piranha.Manager.Menu.Where(m => m.InternalId == "Settings").FirstOrDefault();
            //idx = 0;

            //menubar.Items.Insert(idx++, new Manager.MenuItem()
            //{
            //    Name = "Metadata Sets",
            //    Action = "index",
            //    Controller = "metadata",
            //    Permission = "ADMIN_CONTENT"
            //    //,SelectedActions = "productlist,productedit"
            //});

            //menubar.Items.Insert(idx++, new Manager.MenuItem()
            //{
            //    Name = "Entity Types",
            //    Action = "index",
            //    Controller = "entitytypes",
            //    Permission = "ADMIN_CONTENT"
            //    //,SelectedActions = "productlist,productedit"
            //});

            //Mr Jan 23 2018 adding tab to manager/system area
            //var systemMenu = Manager.Menu.Where(m => m.InternalId == "System").FirstOrDefault();
            //idx = 0;

            //systemMenu.Items.Insert(idx++, new Manager.MenuItem
            //{

            //    Name = "User List",
            //    Action = "index",
            //    Controller = "UserLists",
            //    Permission = "ADMIN_CONTENT"
            //});

            //systemMenu.Items.Insert(systemMenu.Items.Count, new Manager.MenuItem
            //{

            //    Name = "Access Definitions",
            //    Action = "index",
            //    Controller = "AccessDefinitions",
            //    Permission = "ADMIN_CONTENT"
            //});
        }
    }
}

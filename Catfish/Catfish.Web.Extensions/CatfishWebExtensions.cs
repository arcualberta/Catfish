
using CatfishExtensions.Interfaces.Auth;
using static CatfishExtensions.Helpers.ICatfishAppConfiguration;
using ARC.Security.Lib.Google.Interfaces;

namespace CatfishWebExtensions
{
    public static class CatfishWebExtensions
    {
        /// <summary>
        /// Adds the CatfishWebExtensions module.
        /// </summary>
        /// <param name="services">The current service collection</param>
        /// <returns>The services</returns>
        public static IServiceCollection AddCatfishWebExtensions(this IServiceCollection services)
        {
            //Add file extension
            Piranha.App.MediaTypes.Documents.Add(".css", "text/css, application/css-stylesheet");
            // Add the CatfishWebExtensions module
            Piranha.App.Modules.Register<Module>();

            // Setup authorization policies
            services.AddAuthorization(o =>
            {
            // CatfishWebExtensions policies
                o.AddPolicy(Permissions.CatfishWebExtensions, policy =>
                {
                    policy.RequireClaim(Permissions.CatfishWebExtensions, Permissions.CatfishWebExtensions);
                });

            // CatfishWebExtensions add policy
                o.AddPolicy(Permissions.CatfishWebExtensionsAdd, policy =>
                {
                    policy.RequireClaim(Permissions.CatfishWebExtensions, Permissions.CatfishWebExtensions);
                    policy.RequireClaim(Permissions.CatfishWebExtensionsAdd, Permissions.CatfishWebExtensionsAdd);
                });

            // CatfishWebExtensions edit policy
                o.AddPolicy(Permissions.CatfishWebExtensionsEdit, policy =>
                {
                    policy.RequireClaim(Permissions.CatfishWebExtensions, Permissions.CatfishWebExtensions);
                    policy.RequireClaim(Permissions.CatfishWebExtensionsEdit, Permissions.CatfishWebExtensionsEdit);
                });

            // CatfishWebExtensions delete policy
                o.AddPolicy(Permissions.CatfishWebExtensionsDelete, policy =>
                {
                    policy.RequireClaim(Permissions.CatfishWebExtensions, Permissions.CatfishWebExtensions);
                    policy.RequireClaim(Permissions.CatfishWebExtensionsDelete, Permissions.CatfishWebExtensionsDelete);
                });
            });

            //services.AddSingleton<ISecurity, CatfishSecurity>();

            //Catfish services
            services.AddScoped<ICatfishUserManager, CatfishUserManager>();
            services.AddScoped<ICatfishSignInManager, CatfishSignInManager>();
            services.AddScoped<IAssetRegistry, AssetRegistry>();
            services.AddScoped<ICatfishAppConfiguration, ReadAppConfiguration>();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //App.Modules.Manager().Scripts.Add("~/test.js");

            // Return the service collection
            return services;
        }

        /// <summary>
        /// Uses the CatfishWebExtensions.
        /// </summary>
        /// <param name="builder">The application builder</param>
        /// <returns>The builder</returns>
        public static IApplicationBuilder UseCatfishWebExtensions(this IApplicationBuilder builder)
        {
            // Manager resources
            App.Modules.Manager().Scripts
               .Add("~/manager/js/css.js");

            //Registering blocks
            RegisterBlocks();

            

            //Google Login
            (builder as WebApplication)?.MapPost("/google", async ([FromBody] string jwt,
                IGoogleJwtAuthentication googleIdentity,
                IConfiguration configuration,
                HttpRequest request,
                ICatfishUserManager catfishUserManager,
                ICatfishSignInManager catfishSignInManager) =>
            {
                //Decode the login result
                var result = await googleIdentity.GetUserLoginResult(jwt);

                if (result.Success)
                {
                    await catfishSignInManager.AuthorizeSuccessfulExternalLogin(result, request.HttpContext);

                    var siteRoot = configuration.GetSection("SiteConfig:SiteUrl").Value;
                    if (string.IsNullOrEmpty(siteRoot))
                        siteRoot = "/";

                    request.HttpContext.Response.Redirect(siteRoot);
                }
            });

            //Sign Out
            (builder as WebApplication)?.MapGet("/logout", async (IConfiguration configuration, HttpRequest request, ICatfishSignInManager catfishSignInManager) =>
            {
                await catfishSignInManager.SignOut(request.HttpContext);
                var siteRoot = configuration.GetSection("SiteConfig:SiteUrl").Value;
                if (string.IsNullOrEmpty(siteRoot))
                    siteRoot = "/";
                request.HttpContext.Response.Redirect(siteRoot);
            });

            //Initializing tenancy
            (builder as WebApplication)?.MapGet("/init", async (IConfiguration configuration, HttpRequest request, ITenantApiProxy tenantApiProxy) =>
            {
                await tenantApiProxy.EnsureTenancy();
                var siteRoot = configuration.GetSection("SiteConfig:SiteUrl").Value;
                if (string.IsNullOrEmpty(siteRoot))
                    siteRoot = "/";
                request.HttpContext.Response.Redirect(siteRoot);
            });

            return builder
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new EmbeddedFileProvider(typeof(Module).Assembly, "CatfishWebExtensions.assets.manager.js"),
                    RequestPath = "/manager/js"
                })
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new EmbeddedFileProvider(typeof(Module).Assembly, "CatfishWebExtensions.assets.manager.images"),
                    RequestPath = "/manager/images"
                })
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new EmbeddedFileProvider(typeof(Module).Assembly, "CatfishWebExtensions.Pages.DisplayTemplates"),
                    RequestPath = "/Pages/DisplayTemplates"
                })
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new EmbeddedFileProvider(typeof(Module).Assembly, "CatfishWebExtensions.assets.public.css"),
                    RequestPath = "/assets/css"
                })
                 .UseStaticFiles(new StaticFileOptions
                 {
                     FileProvider = new EmbeddedFileProvider(typeof(Module).Assembly, "CatfishWebExtensions.assets.public.vendors.pinia"),
                     RequestPath = "/assets/public/vendors/pinia"
                 })
                 .UseStaticFiles(new StaticFileOptions
                 {
                     FileProvider = new EmbeddedFileProvider(typeof(Module).Assembly, "CatfishWebExtensions.assets.public.vendors.vue3"),
                     RequestPath = "/assets/public/vendors/vue3"
                 })
                 .UseStaticFiles(new StaticFileOptions
                 {
                     FileProvider = new EmbeddedFileProvider(typeof(Module).Assembly, "CatfishWebExtensions.assets.public.vendors.vuedemi"),
                     RequestPath = "/assets/public/vendors/vuedemi"
                 })
                 .UseStaticFiles(new StaticFileOptions
                 {
                     FileProvider = new EmbeddedFileProvider(typeof(Module).Assembly, "CatfishWebExtensions.assets.public.vendors.vuerouter"),
                     RequestPath = "/assets/public/vendors/vuerouter"
                 });                 ;
        }


        /// <summary>
        /// Static accessor to CatfishWebExtensions module if it is registered in the Piranha application.
        /// </summary>
        /// <param name="modules">The available modules</param>
        /// <returns>The CatfishWebExtensions module</returns>
        public static Module CatfishWebExtensionsModule(this Piranha.Runtime.AppModuleList modules)
        {
            return modules.Get<Module>();
        }


        #region Private methods
        private static void RegisterBlocks()
        {
            App.Blocks.Register<ExtendedImage>();
            App.Blocks.Register<Accordion>();
            App.Blocks.Register<Card>();
            App.Blocks.Register<GoogleCalendar>();
            App.Blocks.Register<FormBuilder>();

            //Carousel
            App.Blocks.Register<Slide>();
            App.Blocks.Register<Carousel>();

            //Archive post
            App.Blocks.Register<ArchivePreview>();

        }

        //private static void RegisterAssets()
        //{
            
        //    var headerAttributes = Assets.GetHeaderTypes();
        //    foreach(var att in headerAttributes)
        //    {
        //        var name = att.Name;
        //        var viewTemplate = att.ViewTemplate;
        //    }
        //    Assets.Headers.Add(new PartialView("DefaultHeder", "Headers/_DefaultHeader"));
        //    Assets.Headers.Add(new PartialView("BiLeveleHeader", "Headers/_BiLevelHeader"));

        //    var footerAttributes = Assets.GetFooterTypes();
        //    foreach (var att in footerAttributes)
        //    {
        //        var name = att.Name;
        //        var viewTemplate = att.ViewTemplate;
        //    }
        //    Assets.Footers.Add(new PartialView("DefaultFooter", "Footers/_DefaultFooter"));
        //}

        
        #endregion
    }
}
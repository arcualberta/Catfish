using Microsoft.AspNetCore.Mvc.Razor;

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


            //Catfish services
            services.AddScoped<ICatfishAppConfiguration, ReadAppConfiguration>();
            services.AddScoped<ICatfishUserManager, CatfishUserManager>();
            services.AddScoped<ICatfishSignInManager, CatfishSignInManager>();
            services.AddScoped<IAssetRegistry, AssetRegistry>();

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
                IGoogleIdentity googleIdentity, 
                IConfiguration configuration, 
                HttpRequest request, 
                ICatfishUserManager catfishUserManager,
                ICatfishSignInManager catfishSignInManager) =>
            {
                try
                {
                    var result = await googleIdentity.GetUserLoginResult(jwt);

                    var user = await catfishUserManager.GetUser(result);
                    if (user == null)
                        throw new CatfishException("Unable to retrieve or create user");

                    //Obtain the list of global roles of the user
                    result.GlobalRoles = await catfishUserManager.GetGlobalRoles(user);

                    bool signInStatus = false;
                    if (bool.TryParse(configuration.GetSection("SiteConfig:IsWebApp").Value, out bool isWebApp) && isWebApp)
                        signInStatus = await catfishSignInManager.SignIn(user, request.HttpContext);
                   
                    return result;
                }
                catch (Exception ex)
                {
                    return new LoginResult();
                }
            });

            //Sign Out
            (builder as WebApplication)?.MapGet("/logout", async (IConfiguration configuration, HttpRequest request, ICatfishSignInManager catfishSignInManager) =>
            {
                await catfishSignInManager.SignOut(request.HttpContext);
                if (bool.TryParse(configuration.GetSection("SiteConfig:IsWebApp").Value, out bool isWebApp) && isWebApp)
                {
                    var siteRoot = configuration.GetSection("SiteConfig:SiteUrl").Value;
                    if (string.IsNullOrEmpty(siteRoot))
                        siteRoot = "/";
                    request.HttpContext.Response.Redirect(siteRoot);
                }
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
                 });
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
        }
        #endregion
    }
}
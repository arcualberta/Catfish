using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Piranha;
using Piranha.AspNetCore;
using CatfishWebExtensions;
using CatfishExtensions.Helpers;
using static CatfishExtensions.Helpers.ICatfishAppConfiguration;
using Microsoft.Extensions.Configuration;

public static class CatfishWebExtensionsExtensions
{
    /// <summary>
    /// Adds the CatfishWebExtensions module.
    /// </summary>
    /// <param name="serviceBuilder"></param>
    /// <returns></returns>
    public static PiranhaServiceBuilder UseCatfishWebExtensions(this PiranhaServiceBuilder serviceBuilder)
    {
        serviceBuilder.Services.AddCatfishWebExtensions();

        return serviceBuilder;
    }

    /// <summary>
    /// Uses the CatfishWebExtensions module.
    /// </summary>
    /// <param name="applicationBuilder">The current application builder</param>
    /// <returns>The builder</returns>
    public static PiranhaApplicationBuilder UseCatfishWebExtensions(this PiranhaApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Builder.UseCatfishWebExtensions();

        return applicationBuilder;
    }

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
             });
       
    }

    /// <summary>
    /// Static accessor to CatfishWebExtensions module if it is registered in the Piranha application.
    /// </summary>
    /// <param name="modules">The available modules</param>
    /// <returns>The CatfishWebExtensions module</returns>
    public static Module CatfishWebExtensions(this Piranha.Runtime.AppModuleList modules)
    {
        return modules.Get<Module>();
    }
}

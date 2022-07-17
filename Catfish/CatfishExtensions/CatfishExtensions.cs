
using CatfishExtensions.Constants;
using CatfishExtensions.Helpers;

namespace CatfishExtensions
{
    public static class CatfishExtensions
    {
        public static WebApplicationBuilder AddCatfishExtensions(this WebApplicationBuilder builder)
        {
            ConfigurationManager configuration = builder.Configuration;
            IServiceCollection services = builder.Services;

            CorsHelper.AddPolicies(configuration, services);

            return builder;
        }

        /// <summary>
        /// Uses catfish extensions in the web application
        /// </summary>
        /// <param name="application">The current web application</param>
        /// <returns>The web application</returns>
        public static WebApplication UseCatfishExtensions(this WebApplication app)
        {
            app.UseCors();

            return app;
        }

    }
}

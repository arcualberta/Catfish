
namespace CatfishExtensions
{
    public static class CatfishExtensions
    {
        /// <summary>
        /// Adds catfish extensios to the service collection
        /// </summary>
        /// <param name="serviceCollection">The current service collection</param>
        /// <param name="dbConnectionString">Optional db connection string. The current service collection</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddCatfishServices(this IServiceCollection serviceCollection)
        {

            return serviceCollection;
        }

        /// <summary>
        /// Uses catfish extensions in the web application
        /// </summary>
        /// <param name="application">The current web application</param>
        /// <returns>The web application</returns>
        public static WebApplication UseCatfishExtensions(this WebApplication application)
        {
            //applicationBuilder.Builder.UseCatfishWebExtensions();

            return application;
        }
    }
}

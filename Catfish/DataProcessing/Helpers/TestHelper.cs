using Catfish.API.Repository;
using DataProcessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Catfish.Test.Helpers
{
    public class TestHelper
    {
        public IServiceProvider Seviceprovider { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public TestHelper()
        {
            //Creating a service collection
            var services = new ServiceCollection();

            //Registering configuration object
            IConfiguration configuration = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource() { Path = "appsettings.test.json" })
                .Build();
            services.AddSingleton(typeof(IConfiguration), configuration);
         
            //Registering application DB Context
            string dbConnectionString = configuration.GetConnectionString("catfish");
            services.AddDbContext<RepoDbContext>(options => options
                .UseSqlServer(dbConnectionString)
                );

            //Registering showtime DB Context
            string showtimeConnectionString = configuration.GetConnectionString("showtime");
            services.AddDbContext<ShowtimeDbContext>(options => options
                .UseSqlServer(showtimeConnectionString)
                );


            //Registering other services

            ////services.AddScoped<SolrService>();
            // Solr services
            string solrString = configuration.GetSection("SolarConfiguration:solrCore").Value;

            //Creating a service provider and assigning it to the member variable so that it can be used by 
            //test methods.
            Seviceprovider = services.BuildServiceProvider();
        }

        public RepoDbContext Db => Seviceprovider.GetService<RepoDbContext>();
        public ShowtimeDbContext ShowtimeDb => Seviceprovider.GetService<ShowtimeDbContext>();
        public IConfiguration Configuration => Seviceprovider.GetService<IConfiguration>();

    }
}

using Catfish.API.Repository;
using Catfish.API.Repository.Services;
using Catfish.API.Repository.Interfaces;
using DataProcessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using MySql.EntityFrameworkCore.Extensions;

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
            string dbConnectionString = configuration.GetConnectionString("RepoConnectionString");
            services.AddDbContext<RepoDbContext>(options => options
                .UseSqlServer(dbConnectionString)
                );

            services.AddEntityFrameworkMySQL().AddDbContext<MySqlMoviesDbContext>(options => {
                options.UseMySQL(configuration.GetConnectionString("mysqlMovies"));
            });

            //Registering other services
            //Registering other services
            services.AddScoped<ISolrService, SolrService>();
            services.AddScoped<IEmailService, EmailService>();

            ////services.AddScoped<SolrService>();
            // Solr services
            string solrString = configuration.GetSection("SolarConfiguration:solrCore").Value;

            //Creating a service provider and assigning it to the member variable so that it can be used by 
            //test methods.
            Seviceprovider = services.BuildServiceProvider();

          
            
        }

        public RepoDbContext Db => Seviceprovider.GetService<RepoDbContext>()!;
        public IConfiguration Configuration => Seviceprovider.GetService<IConfiguration>()!;
        public ISolrService Solr => Seviceprovider.GetService<ISolrService>()!;
        public MySqlMoviesDbContext MySqlMoviesDbContext => Seviceprovider.GetService<MySqlMoviesDbContext>()!;

    }
}

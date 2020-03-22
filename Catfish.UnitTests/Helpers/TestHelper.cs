using Catfish.Core.Models;
using Catfish.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Catfish.Tests.Helpers
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
            services.AddDbContext<AppDbContext>(options => options
                .UseSqlServer(dbConnectionString)
                );

            //Registering other services
            services.AddScoped<SeedingService>();
            services.AddScoped<DbEntityService>();
            ////services.AddScoped<SolrService>();


            //Creating a service provider and assigning it to the member variable so that it can be used by 
            //test methods.
            Seviceprovider = services.BuildServiceProvider();
        }

        public AppDbContext Db
        {
            get => Seviceprovider.GetService<AppDbContext>();
        }
    }
}

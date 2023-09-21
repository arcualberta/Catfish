using Catfish.API.Repository;
using Catfish.API.Repository.Services;
using Catfish.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using MySql.EntityFrameworkCore.Extensions;
using DataProcessing.ShowtimeMySqlProcessing;

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

            services.AddEntityFrameworkMySQL().AddDbContext<MySqlCountryOriginDbContext>(options => {
                options.UseMySQL(configuration.GetConnectionString("mysqlCountryOrigins"));
            });

            services.AddEntityFrameworkMySQL().AddDbContext<MySqlDistributionDbContext>(options => {
                options.UseMySQL(configuration.GetConnectionString("mysqlDistributions"));
            });

            services.AddEntityFrameworkMySQL().AddDbContext<MySqlMoviesDbContext>(options => {
                options.UseMySQL(configuration.GetConnectionString("mysqlMovies"));
            });

            services.AddEntityFrameworkMySQL().AddDbContext<MySqlMovieCastDbContext>(options => {
                options.UseMySQL(configuration.GetConnectionString("mysqlMovieCasts"));
            });

            services.AddEntityFrameworkMySQL().AddDbContext<MySqlMovieGenreDbContext>(options => {
                options.UseMySQL(configuration.GetConnectionString("mysqlMovieGenres"));
            });

            services.AddEntityFrameworkMySQL().AddDbContext<MySqlTheaterDbContext>(options => {
                options.UseMySQL(configuration.GetConnectionString("mysqlTheaters"));
            });

            services.AddEntityFrameworkMySQL().AddDbContext<MySqlShowtimeDbContext>(options => {
                options.UseMySQL(configuration.GetConnectionString("mysqlShowtimes"));
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

        public void SetMySqlConnectionTimeouts(int timeoutInMinutes)
        {

            countryDbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(timeoutInMinutes));
            distributionDbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(timeoutInMinutes));
            movieDbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(timeoutInMinutes));
            movieCastDbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(timeoutInMinutes));
            movieGenreDbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(timeoutInMinutes));
            theaterDbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(timeoutInMinutes));
        }



        public RepoDbContext Db => Seviceprovider.GetService<RepoDbContext>()!;
        public IConfiguration Configuration => Seviceprovider.GetService<IConfiguration>()!;
        public ISolrService Solr => Seviceprovider.GetService<ISolrService>()!;
        public MySqlCountryOriginDbContext countryDbContext => Seviceprovider.GetService<MySqlCountryOriginDbContext>()!;
        public MySqlDistributionDbContext distributionDbContext => Seviceprovider.GetService<MySqlDistributionDbContext>()!;
        public MySqlMoviesDbContext movieDbContext => Seviceprovider.GetService<MySqlMoviesDbContext>()!;
        public MySqlMovieCastDbContext movieCastDbContext => Seviceprovider.GetService<MySqlMovieCastDbContext>()!;
        public MySqlMovieGenreDbContext movieGenreDbContext => Seviceprovider.GetService<MySqlMovieGenreDbContext>()!;
        public MySqlTheaterDbContext theaterDbContext => Seviceprovider.GetService<MySqlTheaterDbContext>()!;
        //public MySqlShowtimeDbContext showtimeDbContext => Seviceprovider.GetService<MySqlShowtimeDbContext>()!;

    }
}

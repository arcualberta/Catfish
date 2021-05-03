using Catfish.Core.Models;
using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Core.Services.Solr;
using Catfish.Services;
using ElmahCore;
using ElmahCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Piranha;
using Piranha.AspNetCore.Identity.SQLServer;
using Piranha.Data.EF.SQLServer;
using Piranha.Services;
using SolrNet;
using System;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using Catfish.Helper;

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

            services.AddPiranhaEF<SQLServerDb>(options =>
                options.UseSqlServer(configuration.GetConnectionString("catfish")));
            services.AddPiranhaIdentityWithSeed<IdentitySQLServerDb>(options =>
                options.UseSqlServer(configuration.GetConnectionString("catfish")));

            //Registering application DB Context
            string dbConnectionString = configuration.GetConnectionString("catfish");
            services.AddDbContext<AppDbContext>(options => options
                .UseSqlServer(dbConnectionString)
                );
            services.AddDbContext<IdentitySQLServerDb>(options => options
                .UseSqlServer(dbConnectionString)
                );

            //Additiona Piranha Services
            services.AddScoped<IApi, Piranha.Api>();
            services.AddScoped<ISiteService, Piranha.Services.SiteService>();
            services.AddScoped<IPageService, Piranha.Services.PageService>();
            services.AddScoped<IParamService, ParamService>();
            services.AddScoped<IMediaService, Piranha.Services.MediaService>();
            services.AddScoped<IStorage, Piranha.Local.FileStorage>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<ISolrService, SolrService>();
            services.AddScoped<ICatfishAppConfiguration, ReadAppConfiguration>();


            //Registering other services
            services.AddScoped<SeedingService>();
            services.AddScoped<DbEntityService>();
            services.AddScoped<IEntityIndexService, EntityIndexService>();
            services.AddTransient<IWorkflowService, WorkflowService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();


            ////services.AddScoped<SolrService>();
            // Solr services
            string solrString = configuration.GetSection("SolarConfiguration:solrCore").Value;

            services.AddSolrNet<SolrEntry>(solrString);
            services.AddScoped<ISolrIndexService<SolrEntry>, SolrIndexService<SolrEntry, ISolrOperations<SolrEntry>>>();


            //Adding an empty mock-up error logger instance to the service. This is to replace the actuall
            //Elmah error-log functionality used in the web application.
            services.AddScoped<ErrorLog, MockupErrorLog>();

            services.AddScoped<IJobService, JobService>();

            //Creating a service provider and assigning it to the member variable so that it can be used by 
            //test methods.
            Seviceprovider = services.BuildServiceProvider();
        }

        public AppDbContext Db => Seviceprovider.GetService<AppDbContext>();
        public IdentitySQLServerDb PiranhaDb => Seviceprovider.GetService<IdentitySQLServerDb>();
        public IWorkflowService WorkflowService => Seviceprovider.GetService<IWorkflowService>();
        public IAuthorizationService AuthorizationService => Seviceprovider.GetService<IAuthorizationService>();
        public IConfiguration Configuration => Seviceprovider.GetService<IConfiguration>();
        public ISolrService SolrService => Seviceprovider.GetService<ISolrService>();
       
        public XElement LoadXml(string fileName)
        {
            string dataRoot = Configuration.GetSection("SchemaPath").Value;
            var path = Path.Combine(dataRoot, fileName);
            if (!File.Exists(path))
                throw new Exception("File not found at " + path);

            XElement xml = XElement.Parse(File.ReadAllText(path));
            return xml;
        }

        public bool RefreshSchema(string fileName, IWorkflowService ws)
        {
            var xml = LoadXml(fileName);
            Guid id = Guid.Parse(xml.Attribute("id").Value);
            string message;
            return ws.UpdateItemTemplateSchema(id, xml.ToString(), out message);
        }

        public void RefreshDatabase()
        {
            var _db = Db;

            //Deleting all entities in the Entity table
            var entities = _db.Entities.ToList();
            _db.Entities.RemoveRange(entities);

            //Deleting all system statuses
            var statuses = _db.SystemStatuses.ToList();
            _db.SystemStatuses.RemoveRange(statuses);

            _db.SaveChanges();

            //Reloading default collection
            _db.Collections.Add(Collection.Parse(LoadXml("collection_1.xml")) as Collection);

            //Reloading Item Templates
            var ws = WorkflowService;
            RefreshSchema("simple_form.xml", ws);
            RefreshSchema("visibleIf_requiredIf.xml", ws);
            RefreshSchema("visibleIf_options.xml", ws);
            RefreshSchema("composite_field.xml", ws);
            RefreshSchema("table_field.xml", ws);
            RefreshSchema("table_field2.xml", ws);
            RefreshSchema("grid_table.xml", ws);
            RefreshSchema("SASform.xml", ws);
            RefreshSchema("covidWeeklyInspection.xml", ws);

            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("simple_form.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("visibleIf_requiredIf.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("visibleIf_options.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("composite_field.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("table_field.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("table_field2.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("grid_table.xml")) as ItemTemplate);
            ////_db.ItemTemplates.Add(ItemTemplate.Parse(LoadXml("SASform.xml")) as ItemTemplate);

            _db.SaveChanges();
        }
    }

    public class MockupErrorLog : ErrorLog
    {
        public override ErrorLogEntry GetError(string id)
        {
            return null;
        }

        public override int GetErrors(int pageIndex, int pageSize, ICollection<ErrorLogEntry> errorEntryList)
        {
            return 0;
        }

        public override string Log(Error error)
        {
            return "";
        }
    }
}

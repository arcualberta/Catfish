namespace Catfish.Core.Migrations
{
    using Catfish.Core.Helpers;
    using Catfish.Core.Services;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    
    internal sealed class Configuration : Configuration<Catfish.Core.Models.CatfishDbContext>
    {

    }

    internal abstract class Configuration<TSource> : DbMigrationsConfiguration<TSource> where TSource : Catfish.Core.Models.CatfishDbContext
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            SetSqlGenerator("System.Data.SqlClient", new XmlSqlServerMigrationSqlGenerator());
        }

        protected override void Seed(TSource context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            string solrString = System.Configuration.ConfigurationManager.AppSettings["SolrServer"];
            SolrService.Init(solrString);
        }
    }
}

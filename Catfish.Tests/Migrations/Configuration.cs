using Catfish.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Sql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations.Model;

namespace Catfish.Tests.Migrations
{

    internal sealed class Configuration : Catfish.Core.Migrations.Configuration<CatfishTestDbContext>
    {

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("System.Data.SQLite", new MigrationSqLiteGenerator());
        }
    }
}

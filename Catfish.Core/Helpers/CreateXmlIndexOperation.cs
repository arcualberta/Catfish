using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Helpers
{
    public class CreateXmlIndexOperation : MigrationOperation
    {
        public string Table { get; set; }
        public string Column { get; set; }
        public string IndexName { get; set; }
        public bool IsPrimary { get; set; }
        public eIndexType IndexType { get; set; }
        public string PrimaryKeyName { get; set; }
        

        public enum eIndexType
        {
            VALUE,
            PATH,
            PROPERTY
        }

        public CreateXmlIndexOperation(string table, string column, string keyName, bool isPrimary = true, eIndexType indexType = eIndexType.VALUE, string primaryKeyName = null)
            : base(null)
        {
            Table = table;
            Column = column;
            IndexName = keyName;
            IsPrimary = isPrimary;
            IndexType = indexType;
            PrimaryKeyName = primaryKeyName;
        }

        public override bool IsDestructiveChange
        {
            get { return false; }
        }

        private void WriteUpAsSqlServer(TextWriter writer)
        {
            writer.WriteLine("CREATE {0} XML INDEX {1} ON {2} ( {3} )",
                IsPrimary ? "PRIMARY" : "",
                IndexName, Table, Column);

            if (!IsPrimary)
            {
                writer.WriteLine("USING XML INDEX {0} FOR {1}", PrimaryKeyName, IndexType.ToString());
            }

            writer.WriteLine(";");
        }

        public void WriteUpAsSql(TextWriter writer, string activeProvider)
        {
            switch (activeProvider)
            {
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    WriteUpAsSqlServer(writer);
                    break;
            }
        }
    }

    public class RemoveXmlIndexOperation : MigrationOperation
    {
        public string KeyName { get; private set; }
        public string Table { get; private set; }

        public RemoveXmlIndexOperation(string table, string keyName) : base(null)
        {
            KeyName = keyName;
            Table = table;
        }

        public override bool IsDestructiveChange
        {
            get { return false; }
        }

        public void WriteUpAsSql(TextWriter writer, string activeProvider)
        {
            writer.WriteLine("DROP INDEX {0} ON {1};", KeyName, Table);
        }
    }

    public static class MigrationXmlExtensions
    {
        public static void CreateXmlIndex(this DbMigration migration, string table, string column, string keyName, bool isPrimary = true, CreateXmlIndexOperation.eIndexType indexType = CreateXmlIndexOperation.eIndexType.VALUE, string primaryKeyName = null)
        {
            ((IDbMigration)migration).AddOperation(new CreateXmlIndexOperation(table, column, keyName, isPrimary, indexType, primaryKeyName));
        }

        public static void RemoveXmlIndex(this DbMigration migration, string table, string keyName)
        {
            ((IDbMigration)migration).AddOperation(new RemoveXmlIndexOperation(table, keyName));
        }
    }

    public class XmlSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(MigrationOperation migrationOperation)
        {
            if (migrationOperation != null)
            {
                Type type = migrationOperation.GetType();

                if (typeof(CreateXmlIndexOperation).IsAssignableFrom(type))
                {
                    using (var writer = Writer())
                    {

                        ((CreateXmlIndexOperation)migrationOperation).WriteUpAsSql(writer, "Microsoft.EntityFrameworkCore.SqlServer");
                        Statement(writer);
                    }
                    return;
                }else if (typeof(RemoveXmlIndexOperation).IsAssignableFrom(type))
                {
                    using (var writer = Writer())
                    {

                        ((RemoveXmlIndexOperation)migrationOperation).WriteUpAsSql(writer, "Microsoft.EntityFrameworkCore.SqlServer");
                        Statement(writer);
                    }
                    return;
                }
            }

            base.Generate(migrationOperation);
        }
    }
}

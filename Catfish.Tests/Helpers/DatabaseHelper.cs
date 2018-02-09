using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Catfish.Core.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using static Catfish.Core.Models.EntityType;
using System.Data.Entity.Core.Common;
using System.Data.SQLite.EF6;
using System.Data.SQLite;
using System.ComponentModel.DataAnnotations.Schema;
using Catfish.Tests.Migrations;
using System.Data.Entity.Migrations;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.Migrations.Sql;

namespace Catfish.Tests.Helpers
{
    class DatabaseHelper
    {
        private CatfishTestDbContext mDb { get; set; }
        public CatfishTestDbContext Db
        {
            get
            {
                if (mDb == null)
                {
                    DbConnection connection = GetConnection(GetConnectionStringByName("piranha"));
                    connection.Open();
                    mDb = new CatfishTestDbContext(connection);
                }

                return mDb;
            }
        }

        private MetadataService mMs { get; set; }
        public MetadataService Ms
        {
            get
            {
                if (mMs == null)
                {
                    mMs = new MetadataService(Db);
                }

                return mMs;
            }
        }

        private EntityService mEs { get; set; }
        public EntityService Es
        {
            get
            {
                if (mEs == null)
                {
                    mEs = new EntityService(Db);
                }

                return mEs;
            }
        }

        private CollectionService mCs { get; set; }
        public CollectionService Cs
        {
            get
            {
                if (mCs == null)
                {
                    mCs = new CollectionService(Db);
                }

                return mCs;
            }
        }

        private ItemService mIs { get; set; }
        public ItemService Is
        {
            get
            {
                if (mIs == null)
                {
                    mIs = new ItemService(Db);
                }

                return mIs;
            }
        }

        public DatabaseHelper(bool reinitializeDb = false)
        {
            if (reinitializeDb)
            {
                ReInitializeDatabase();
            }
        }

        private ConnectionStringSettings GetConnectionStringByName(string name)
        {
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            if(settings != null)
            {
                foreach(ConnectionStringSettings s in settings)
                {
                    if(s.Name == name)
                    {
                        return s;
                    }
                }
            }

            return null;
        }

        private DbConnection GetConnection(ConnectionStringSettings settings)
        {
            DbConnection connection = null;

            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(settings.ProviderName);
                connection = factory.CreateConnection();
                connection.ConnectionString = settings.ConnectionString;
            }catch(Exception ex)
            {
                throw ex;
            }

            return connection;
        }

        private void CreateMetadata()
        {
            MetadataSet metadata = new MetadataSet();
            metadata.SetName("Basic Metadata");
            metadata.SetDescription("Metadata Description");

            List<FormField> fields = new List<FormField>();

            FormField field = new TextField();
            field.Name = "Name";
            field.Description = "The Description";
            fields.Add(field);

            field = new TextField();
            field.Name = "Description";
            field.Description = "Description Description";
            fields.Add(field);

            metadata.Fields = fields;
            metadata.Serialize();

            Ms.UpdateMetadataSet(metadata);

            Db.SaveChanges();
        }

        private void CreateEntityTypes()
        {
            MetadataSet metadata = Ms.GetMetadataSets().FirstOrDefault();

            for (int i = 0; i < 10; ++i)
            {
                EntityType et = new EntityType();
                et.Name = "Entity" + (i + 1);
                et.MetadataSets.Add(metadata);

                List<eTarget> targets = new List<eTarget>();

                if (i % 2 == 0)
                {
                    targets.Add(eTarget.Items);
                }

                if (i % 5 == 1)
                {
                    targets.Add(eTarget.Collections);
                }

                et.TargetTypesList = targets;

                et.AttributeMappings.Add(new EntityTypeAttributeMapping()
                {
                    Name = "Name Mapping",
                    MetadataSet = metadata,
                    FieldName = "Name"
                });

                et.AttributeMappings.Add(new EntityTypeAttributeMapping()
                {
                    Name = "Description Mapping",
                    MetadataSet = metadata,
                    FieldName = "Description"
                });

                Db.EntityTypes.Add(et);
            }

            Db.SaveChanges();
        }

        private void CreateCollections()
        {
            List<int> ets = Es.GetEntityTypes().ToList().Where(et => et.TargetTypesList.Contains(eTarget.Collections)).Select(et => et.Id).ToList();

            for (int i = 0; i < 10; ++i)
            {
                int index = i % ets.Count;
                Collection c = Cs.CreateEntity<Collection>(ets[index]);
                c.SetName("Collection " + (i + 1));
                c.SetDescription("Description for Collection " + (i + 1));

                c.Serialize();
                Cs.UpdateStoredCollection(c);
            }

            Db.SaveChanges();
        }

        private void CreateItems()
        {
            List<int> ets = Es.GetEntityTypes().ToArray().Where(et => et.TargetTypesList.Contains(eTarget.Items)).Select(et => et.Id).ToList();

            for (int i = 0; i < 10; ++i)
            {
                int index = i % ets.Count;
                Item e = Is.CreateEntity<Item>(ets[index]);
                e.SetName("Item " + (i + 1));
                e.SetDescription("Description for Item " + (i + 1));

                e.Serialize();
                Is.UpdateStoredItem(e);
            }

            Db.SaveChanges();
        }

        private void SetupData()
        {
            CreateMetadata();
            CreateEntityTypes();
            CreateCollections();
            CreateItems();
        }

        public void Initialize()
        {
            Catfish.Tests.Migrations.Configuration config = new Catfish.Tests.Migrations.Configuration();
            var migrator = new DbMigrator(config);
            
            foreach(string migName in migrator.GetPendingMigrations())
            {
                Type migration = config.MigrationsAssembly.GetType(string.Format("{0}.{1}", config.MigrationsNamespace, migName.Substring(16)));
                DbMigration m = (DbMigration)Activator.CreateInstance(migration);
                m.Up();

                var prop = m.GetType().GetProperty("Operations", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if(prop != null)
                {
                    IEnumerable<MigrationOperation> operations = prop.GetValue(m) as IEnumerable<MigrationOperation>;
                    var generator = config.GetSqlGenerator("System.Data.SQLite");
                    var statements = generator.Generate(operations, "2008");
                    foreach (MigrationStatement item in statements)
                        Db.Database.ExecuteSqlCommand(item.Sql);
                }
                
            }
        }

        public void ReInitializeDatabase()
        {
            Initialize();

            //SetupPiranha();
            SetupData();
            var test = Db.MetadataSets.ToArray();
        }
    }

    public class SqliteConfiguration : DbConfiguration
    {
        public SqliteConfiguration()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }
    }
    
    public class CatfishTestDbContext : CatfishDbContext
    {
        public CatfishTestDbContext() : base(){

        }

        public CatfishTestDbContext(DbConnection connection) : base(connection, true)
        {
        }

        protected override void SetColumnTypes(DbModelBuilder builder)
        {
            builder.HasDefaultSchema("");

            builder.Entity<XmlModel>().Property(xm => xm.Content).HasColumnType("");
        }
    }
}

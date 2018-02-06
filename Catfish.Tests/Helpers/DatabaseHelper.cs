using Catfish.Core.Models;
using Catfish.Core.Models.Forms;
using Catfish.Core.Services;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Catfish.Core.Models.EntityType;

namespace Catfish.Tests.Helpers
{
    class DatabaseHelper
    {
        private CatfishDbContext mDb { get; set; }
        public CatfishDbContext Db
        {
            get
            {
                if (mDb == null)
                {
                    //System.Data.Sql
                    var connection = new SqliteConnection("DataSource=:memory:");
                    mDb = new CatfishDbContext();
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
            List<int> ets = Es.GetEntityTypes().Where(et => et.TargetTypesList.Contains(eTarget.Collections)).Select(et => et.Id).ToList();

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
            List<int> ets = Es.GetEntityTypes().Where(et => et.TargetTypesList.Contains(eTarget.Items)).Select(et => et.Id).ToList();

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

        public void ReInitializeDatabase()
        {
            Db.Database.Initialize(true);

            // This is done because several Proxy objects are created on initialization and must be removed.
            Db.MetadataSets.RemoveRange(Db.MetadataSets.ToArray());
            Db.EntityTypes.RemoveRange(Db.EntityTypes.ToArray());
            Db.Entities.RemoveRange(Db.Entities.ToArray());
            Db.SaveChanges();

            var test = Db.MetadataSets.ToArray();
            //SetupPiranha();
            SetupData();
        }
    }
}

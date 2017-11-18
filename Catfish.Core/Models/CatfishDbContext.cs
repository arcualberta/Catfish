using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Catfish.Core.Models.Forms;
using Catfish.Core.Models.Data;

namespace Catfish.Core.Models
{
    public class CatfishDbContext : DbContext
    {
        public CatfishDbContext()
            : base("piranha")
        {

        }

        ////private static CatfishDbContext mDb;
        ////public static CatfishDbContext Instance
        ////{
        ////    get
        ////    {
        ////        if (mDb == null)
        ////            mDb = new CatfishDbContext();
        ////        return mDb;
        ////    }
        ////}

        ////private static Piranha.DataContext mPiranhaDb;
        ////public static Piranha.DataContext PiranhaInstance
        ////{
        ////    get
        ////    {
        ////        if (mPiranhaDb == null)
        ////            mPiranhaDb = new Piranha.DataContext();
        ////        return mPiranhaDb;
        ////    }
        ////}

        public override int SaveChanges()
        {
            if (this.ChangeTracker.HasChanges())
            {
                foreach(var entry in this.ChangeTracker.Entries<XmlModel>())
                {
                    if (entry.State != EntityState.Unchanged && entry.State != EntityState.Deleted)
                    {
                        entry.Entity.Serialize();
                    }
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Aggregation>()
                .HasMany<Aggregation>(p => p.ChildMembers)
                .WithMany(c => c.ParentMembers)
                .Map(t =>
                {
                    t.MapLeftKey("ParentId");
                    t.MapRightKey("ChildId");
                    t.ToTable("AggregationHasMembers");
                });

            builder.Entity<Aggregation>()
                .HasMany<Item>(p => p.ChildRelations)
                .WithMany(c => c.ParentRelations)
                .Map(t =>
                {
                    t.MapLeftKey("ParentId");
                    t.MapRightKey("ChildId");
                    t.ToTable("AggregationHasRelatedObjects");
                });

            builder.Entity<EntityType>()
                .HasMany<MetadataSet>(et => et.MetadataSets)
                .WithMany(ms => ms.EntityTypes)
                .Map(t =>
                {
                    t.MapLeftKey("EntityTypesId");
                    t.MapRightKey("MetadataSetId");
                    //t.MapLeftKey("MetadataSetId");
                    //t.MapRightKey("EntityTypesId");
                    t.ToTable("EntityTypeHasMetadataSets");
                });
        }

        public DbSet<XmlModel> XmlModels { get; set; }

 //      public DbSet<Entity> Entities { get; set; }

        public DbSet<Collection> Collections { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<EntityType> EntityTypes { get; set; }

        public DbSet<EntityTypeAttributeMapping> EntityTypeAttributeMappings { get; set; }

        public DbSet<MetadataSet> MetadataSets { get; set; }

        public DbSet<Form> FormTemplates { get; set; }
 
        ////public DbSet<SimpleField> MetadataFields { get; set; }

        ////public DbSet<FieldValue> FieldValues { get; set; }
    }
}
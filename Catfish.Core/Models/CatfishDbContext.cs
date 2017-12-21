using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Catfish.Core.Models.Forms;
using Catfish.Core.Models.Data;
using System.Security.Principal;

namespace Catfish.Core.Models
{
    public class CatfishDbContext : DbContext
    {
        public CatfishDbContext()
            : base("piranha")
        {

        }

        public int SaveChanges(IIdentity actor)
        {
            if (actor.IsAuthenticated)
                return SaveChanges(actor.Name);
            else
                return SaveChanges("Annonymous");
        }
        public int SaveChanges(string actor)
        {
            if (this.ChangeTracker.HasChanges())
            {
                foreach(var entry in this.ChangeTracker.Entries<XmlModel>())
                {
                    if (entry.State != EntityState.Unchanged && entry.State != EntityState.Deleted)
                    {
                        AuditEntry.eAction action = entry.Entity.Id == 0 ? AuditEntry.eAction.Create : AuditEntry.eAction.Update;
                        entry.Entity.FlushChangeLog(action, actor);
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

        public System.Data.Entity.DbSet<Catfish.Core.Models.Forms.TextField> TextFields { get; set; }

        ////public DbSet<SimpleField> MetadataFields { get; set; }

        ////public DbSet<FieldValue> FieldValues { get; set; }
    }
}
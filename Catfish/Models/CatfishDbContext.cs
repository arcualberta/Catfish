using Catfish.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Catfish.Models
{
    public class CatfishDbContext : DbContext
    {
        public CatfishDbContext()
            :base("piranha")
        {

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
                .HasMany<DigitalObject>(p => p.ChildRelations)
                .WithMany(c => c.ParentRelations)
                .Map(t =>
                {
                    t.MapLeftKey("ParentId");
                    t.MapRightKey("ChildId");
                    t.ToTable("AggregationHasRelatedObjects");
                });

        }

        public DbSet<DigitalEntity> DigitalEntities { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<DigitalObject> DigitalObject { get; set; }

    }
}
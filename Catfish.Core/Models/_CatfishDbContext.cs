using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models
{
    public class CatfishDbContext : DbContext
    {
        /// <summary>
        /// The application config.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        public CatfishDbContext(DbContextOptions<CatfishDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("catfish"));
            }
        }

        ////public ObjectContext ObjectContext
        ////{
        ////    get
        ////    {
        ////        return ((IObjectContextAdapter)this).ObjectContext;
        ////    }
        ////}

        ////public void Detach(object entity)
        ////{
        ////    ObjectContext.Detach(entity);
        ////}


        protected override void OnModelCreating(ModelBuilder builder)
        {
 //           builder.Entity<AggregationHasMembers>()
 //               .HasKey(c => new { c.ParentId, c.ChildId });
            
            //builder.Entity<Aggregation>()
            //    .HasMany<Item>(p => p.ManagedRelatedMembers)
            //    .WithMany(c => c.ParentRelations)
            //    .Map(t =>
            //    {
            //        t.MapLeftKey("ParentId");
            //        t.MapRightKey("ChildId");
            //        t.ToTable("AggregationHasRelatedObjects");
            //    });

            //builder.Entity<EntityType>()
            //    .HasMany<MetadataSet>(et => et.MetadataSets)
            //    .WithMany(ms => ms.EntityTypes)
            //    .Map(t =>
            //    {
            //        t.MapLeftKey("EntityTypesId");
            //        t.MapRightKey("MetadataSetId");
            //        t.ToTable("EntityTypeHasMetadataSets");
            //    });

            //define composite primary key for EntityGroupUser -- Jan 24 2018
            ////builder.Entity<CFUserListEntry>().HasKey(t => new { t.CFUserListId, t.UserId });
        }

        public DbSet<XmlModel> XmlModels { get; set; }
        public DbSet<Collection2> Collections { get; set; }
        public DbSet<Item2> Items { get; set; }


/*
        public DbSet<XmlModel> XmlModels { get; set; }

        public DbSet<Entity> Entities { get; set; }
        public DbSet<EntityType> EntityTypes { get; set; }

        public DbSet<MetadataSet> MetadataSets { get; set; }

        public DbSet<EntityTypeAttributeMapping> EntityTypeAttributeMappings { get; set; }

        public DbSet<Aggregation> Aggregations { get; set; }

        public DbSet<Collection> Collections { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Relation> Relations { get; set; }
*/

        //        public DbSet<CFAccessDefinition> AccessDefinitions { get; set; }
        /*

                public DbSet<Form> FormTemplates { get; set; }

                public System.Data.Entity.DbSet<Catfish.Core.Models.Forms.TextField> TextFields { get; set; }

                public DbSet<CFUserList> UserLists { get; set; }
                public DbSet<CFUserListEntry> UserListEntries { get; set; }

                public DbSet<CFAccessDefinition> AccessDefinitions { get; set; }


         * 
         */


    }
}

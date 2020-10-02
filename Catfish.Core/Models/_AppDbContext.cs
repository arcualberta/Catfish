using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Core.Services.Solr;
﻿using Catfish.Core.Models.Contents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Piranha.AspNetCore.Identity.SQLServer;
   
namespace Catfish.Core.Models
{
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// The application config.
        /// </summary>
        public IConfiguration Configuration { get; set; }

        //public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
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

        
        public override int SaveChanges()
        {
            return base.SaveChanges();
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
            base.OnModelCreating(builder);

            #region Defining many-to-many named relationship of Aggregation with other Aggregations
            // Reference: https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
            builder.Entity<Relationship>()
                .HasKey(rel => new { rel.SubjectId, rel.ObjctId });

            builder.Entity<Relationship>()
                .HasOne(rel => rel.Subject)
                .WithMany(sub => sub.SubjectRelationships)
                .HasForeignKey(rel => rel.SubjectId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Relationship>()
                .HasOne(rel => rel.Objct)
                .WithMany(obj => obj.ObjectRelationships)
                .HasForeignKey(rel => rel.ObjctId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion Defining many-to-many named relationship of Aggregation with other Aggregations

        }

        public DbSet<Entity> Entities { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<EntityTemplate> EntityTemplates { get; set; }
        public DbSet<ItemTemplate> ItemTemplates { get; set; }
        public DbSet<CollectionTemplate> CollectionTemplates { get; set; }
        public DbSet<SystemPage> SystemPages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroupRole> UserGroups { get; set; }
        public DbSet<GroupRole> GroupRoles { get; set; }
        public DbSet<GroupTemplate> GroupTemplates { get; set; }
        public DbSet<Form> Forms { get; set; }

        /*
                public DbSet<XmlModel> XmlModels { get; set; }
                public DbSet<XmlModel> XmlModels { get; set; }

                public DbSet<Entity> Entities { get; set; }
                public DbSet<EntityType> EntityTypes { get; set; }
                public DbSet<MetadataSet> MetadataSets { get; set; }
                public DbSet<Form> Forms { get; set; }

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

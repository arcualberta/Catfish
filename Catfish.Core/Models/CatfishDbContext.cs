using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Catfish.Core.Models.Forms;
using Catfish.Core.Models.Data;
using System.Security.Principal;
using Catfish.Core.Models.Access;
using Catfish.Core.Services;
using CommonServiceLocator;
using SolrNet;
using System.Configuration;
using System.Data.Entity.Infrastructure;

namespace Catfish.Core.Models
{
    public class CatfishDbContext : DbContext
    {
        private ISolrOperations<Dictionary<string, object>> solr { get; set; }

        public CatfishDbContext()
            : base("piranha")
        {
            if (SolrService.IsInitialized)
            {
                solr = ServiceLocator.Current.GetInstance<ISolrOperations<Dictionary<string, object>>>();
            }

            var sqlTimeout = ConfigurationManager.AppSettings["SqlConnectionTimeoutSeconds"];
            if (!string.IsNullOrEmpty(sqlTimeout))
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = int.Parse(sqlTimeout);
        }

        public CatfishDbContext(System.Data.Common.DbConnection connection, bool contextOwnsConnection) : base(connection, contextOwnsConnection)
        {
            solr = ServiceLocator.Current.GetInstance<ISolrOperations<Dictionary<string, object>>>();
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
                foreach(var entry in this.ChangeTracker.Entries<CFXmlModel>())
                {
                    if (entry.State != EntityState.Unchanged && entry.State != EntityState.Deleted)
                    {
                        CFAuditEntry.eAction action = entry.Entity.Id == 0 ? CFAuditEntry.eAction.Create : CFAuditEntry.eAction.Update;
                        entry.Entity.FlushChangeLog(action, actor);
                        entry.Entity.Serialize();
                    }
                }
            }

            return base.SaveChanges();
        }

        /**
         * Used to define column types that may not be available in all Databases.
         **/
        protected virtual void SetColumnTypes(DbModelBuilder builder)
        {
            builder.Entity<CFXmlModel>().Property(xm => xm.Content).HasColumnType("xml");
        }
        
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            SetColumnTypes(builder);

            builder.Entity<CFAggregation>()
                .HasMany<CFAggregation>(p => p.ManagedChildMembers)
                .WithMany(c => c.ManagedParentMembers)
                .Map(t =>
                {
                    t.MapLeftKey("ParentId");
                    t.MapRightKey("ChildId");
                    t.ToTable("AggregationHasMembers");
                });

            builder.Entity<CFAggregation>()
                .HasMany<CFItem>(p => p.ChildRelations)
                .WithMany(c => c.ParentRelations)
                .Map(t =>
                {
                    t.MapLeftKey("ParentId");
                    t.MapRightKey("ChildId");
                    t.ToTable("AggregationHasRelatedObjects");
                });

            builder.Entity<CFEntityType>()
                .HasMany<CFMetadataSet>(et => et.MetadataSets)
                .WithMany(ms => ms.EntityTypes)
                .Map(t =>
                {
                    t.MapLeftKey("EntityTypesId");
                    t.MapRightKey("MetadataSetId");
                    t.ToTable("EntityTypeHasMetadataSets");
                });

            //define composite primary key for EntityGroupUser -- Jan 24 2018
            builder.Entity<CFUserListEntry>().HasKey(t => new { t.CFUserListId, t.UserId });
        }

        public DbSet<CFXmlModel> XmlModels { get; set; }

       public DbSet<CFEntity> Entities { get; set; }

        public DbSet<CFCollection> Collections { get; set; }

        public DbSet<CFItem> Items { get; set; }

        public DbSet<CFEntityType> EntityTypes { get; set; }

        public DbSet<CFEntityTypeAttributeMapping> EntityTypeAttributeMappings { get; set; }

        public DbSet<CFMetadataSet> MetadataSets { get; set; }

        public DbSet<Form> FormTemplates { get; set; }

        public System.Data.Entity.DbSet<Catfish.Core.Models.Forms.TextField> TextFields { get; set; }

        public DbSet<CFUserList> UserLists { get; set; }
        public DbSet<CFUserListEntry> UserListEntries { get; set; }

        public DbSet<CFAccessDefinition> AccessDefinitions { get; set; }

        public System.Data.Entity.DbSet<Catfish.Core.Models.Forms.CompositeFormField> CFXmlModels { get; set; }
    }
}
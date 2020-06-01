using Catfish.Core.Models.Solr;
using Catfish.Core.Services;
using Catfish.Core.Services.Solr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catfish.Core.Models
{
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// The application config.
        /// </summary>
        public IConfiguration Configuration { get; set; }

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

        private void UpdateSolr()
        {
            //string solrString = System.Configuration.ConfigurationManager.AppSettings["SolrServer"];

            List<Dictionary<string, object>> savedEntities = new List<Dictionary<string, object>>();
            List<string> deletedEntities = new List<string>();

            //SolrIndexService.AddUpdate(new SolrItemModel(savedEntities));

            //foreach (DbEntityEntry entry in ChangeTracker.Entries<CFEntity>())
            //{

            //    //entry.GetType().InvokeMember("ToSolrDictionary")
            //    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            //    {
            //        savedEntities.Add(((CFEntity)entry.Entity).ToSolrDictionary());
            //    }
            //    else if (entry.State == EntityState.Deleted)
            //    {
            //        deletedEntities.Add(((CFEntity)entry.Entity).Guid);
            //    }
            //}

            //SolrService.SolrOperations.AddRange(savedEntities);
            //SolrService.SolrOperations.Delete(deletedEntities);




        }


        public override int SaveChanges()
        {
            UpdateSolr();
            return base.SaveChanges();

            ////////using (DbContextTransaction dbContextTransaction = Database.BeginTransaction())
            ////////{
            ////////    try
            ////////    {
            ////////        UpdateSolr();
            ////////        int result = base.SaveChanges();
            ////////        dbContextTransaction.Commit();
            ////////        SolrService.SolrOperations.Commit();
            ////////        return result;
            ////////    }
            ////////    catch (SolrNetException e)
            ////////    {
            ////////        // rollback savechanges
            ////////        dbContextTransaction.Rollback();
            ////////        throw e;
            ////////    }
            ////////    catch (Exception e)
            ////////    {
            ////////        throw e;
            ////////    }
            ////////}
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

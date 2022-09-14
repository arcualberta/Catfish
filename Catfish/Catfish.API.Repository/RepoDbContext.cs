
namespace Catfish.API.Repository
{
    public class RepoDbContext : DbContext
    {
        public RepoDbContext(DbContextOptions<RepoDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            DbHelper.SetTablePrefix(builder, "CF_Repo_");

            builder.Entity<EntityTemplate>()
                .HasDiscriminator<string>("Discrimiinator");

            builder.Entity<EntityTemplate>()
                .ToTable("CF_Repo_EntityTemplates");

            builder.Entity<ItemTemplate>()
                .ToTable("CF_Repo_EntityTemplates");

            builder.Entity<CollectionTemplate>()
                .ToTable("CF_Repo_EntityTemplates");

            builder.Entity<Entity>()
                .HasDiscriminator<string>("Discrimiinator");

            builder.Entity<Item>()
                .ToTable("CF_Repo_Entities");

            builder.Entity<Collection>()
                .ToTable("CF_Repo_Entities");


            builder.Entity<SubjectRelationship>()
                .HasKey(x => new { x.SubjectEntityId, x.ObjectEntityId/*, x.SubjectCollectionId, x.ObjectCollectionId*/ });

            builder.Entity<ObjectRelationship>()
                .HasKey(x => new { x.SubjectEntityId, x.ObjectEntityId/*, x.SubjectCollectionId, x.ObjectCollectionId*/ });

            //builder.Entity<Relationship>()
            //                .HasMany<Entity>(p => p.s)
            //                .WithMany(c => c.ParentRelations)
            //                .Map(t =>
            //                {
            //                    t.MapLeftKey("ParentId");
            //                    t.MapRightKey("ChildId");
            //                    t.ToTable("AggregationHasRelatedObjects");
            //                });


            ////// Many-to-many "named" relationship between entities and other entities.
            ////// Reference: https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#many-to-many
            //////
            //// builder.Entity<Entity>()
            ////.HasMany(p => p.RelatedEntities)
            ////.WithMany(p => p.RelatedEntities)
            ////.UsingEntity<Relationship>(
            ////    j => j
            ////        .HasOne(relationship => relationship.SubjectEntity)
            ////        .WithMany(entity => entity.Relationships)
            ////        .HasForeignKey(relationship => relationship.SubjectEntityId),
            ////    j => j
            ////        .HasOne(relationship => relationship.ObjectEntity)
            ////        .WithMany(entity => entity.Relationships)
            ////        .HasForeignKey(relationship => relationship.ObjectEntityId),
            ////    j =>
            ////    {
            ////        j.Property(relationship => relationship.Predicate).HasDefaultValue(RelationshipType.Child);
            ////        j.HasKey(relationship => new { relationship.SubjectEntityId, relationship.ObjectEntityId });
            ////    });
        }

        public DbSet<Form>? Forms { get; set; }
        public DbSet<FormData>? FormData { get; set; }
        public DbSet<ItemTemplate>? ItemTemplates { get; set; }
        public DbSet<CollectionTemplate>? CollectionTemplate { get; set; }
        public DbSet<Entity>? Entities { get; set; }
        public DbSet<Item>? Items { get; set; }
        public DbSet<Collection>? Collections { get; set; }

        public DbSet<SubjectRelationship>? SubjectRelationships { get; set; }
        public DbSet<ObjectRelationship>? ObjectRelationships { get; set; }
    }
}
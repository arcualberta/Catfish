
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

            // Many-to-many "named" relationship between entities and other entities.
            // Reference: https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#many-to-many
            //
            builder.Entity<Entity>()
           .HasMany(p => p.ObjectEntities)
           .WithMany(p => p.SubjectEntities)
           .UsingEntity<Relationship>(
               j => j
                   .HasOne(relationship => relationship.SubjectEntity)
                   .WithMany(entity => entity.Relationships)
                   .HasForeignKey(relationship => relationship.SubjectEntityId),
               j => j
                   .HasOne(relationship => relationship.ObjectEntity)
                   .WithMany(entity => entity.Relationships)
                   .HasForeignKey(relationship => relationship.ObjectEntityId),
               j =>
               {
                   j.Property(relationship => relationship.Predicate).HasDefaultValue(RelationshipType.Child);
                   j.HasKey(relationship => new { relationship.SubjectEntityId, relationship.ObjectEntityId });
               });
        }

        public DbSet<Form>? Forms { get; set; }
        public DbSet<FormData>? FormData { get; set; }
        public DbSet<EntityTemplate>? EntityTemplates { get; set; }
        public DbSet<ItemTemplate>? ItemTemplates { get; set; }
        public DbSet<CollectionTemplate>? CollectionTemplate { get; set; }
        public DbSet<Relationship>? Relationships { get; set; }
    }
}
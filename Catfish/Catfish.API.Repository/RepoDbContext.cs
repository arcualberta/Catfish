
using Catfish.API.Repository.Models.Workflow;

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
                .ToTable("CF_Repo_EntityTemplates");

            //builder.Entity<Entity>()
            //    .HasDiscriminator<string>("Discrimiinator");

            builder.Entity<EntityData>()
                .ToTable("CF_Repo_Entities");

            //builder.Entity<Item>()
            //    .ToTable("CF_Repo_Entities");

            //builder.Entity<Collection>()
            //    .ToTable("CF_Repo_Entities");

            //Configuring the two one-to-many association (SubjectRelationships and ObjectRelationships)
            //between Entity and Relationship models.
            //Reference: https://stackoverflow.com/questions/49214748/many-to-many-self-referencing-relationship/49219124#49219124
            builder.Entity<Relationship>()
                 .HasOne(pt => pt.ObjectEntity)
                 .WithMany(p => p.ObjectRelationships)
                 .HasForeignKey(pt => pt.ObjectEntityId)
                 .OnDelete(DeleteBehavior.Restrict); // Avoid cascade-delete behaviour in one of the two parallel associations between Entity and Relationship

            builder.Entity<Relationship>()
                .HasOne(pt => pt.SubjectEntity)
                .WithMany(t => t.SubjectRelationships)
                .HasForeignKey(pt => pt.SubjectEntityId);

        }

        public DbSet<FormTemplate>? Forms { get; set; }
        public DbSet<FormData>? FormData { get; set; }
        public DbSet<EntityData>? Entities { get; set; }
        public DbSet<EntityTemplate>? EntityTemplates { get; set; }
        //public DbSet<Collection>? Collections { get; set; }
        public DbSet<Relationship>? Relationships { get; set; }
        public DbSet<WorkflowDbRecord> Workflows { get; set; }

    }
}

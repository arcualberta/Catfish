
namespace Catfish.API.Repository
{
    public class RepoDbContext: DbContext
    {
        public RepoDbContext(DbContextOptions<RepoDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

            DbHelper.SetTablePrefix(builder, "CF_Repo_");
		}

		public DbSet<Form>? Forms { get; set; }
	}
}

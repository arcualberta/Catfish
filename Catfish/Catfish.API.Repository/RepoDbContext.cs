
namespace Catfish.API.Repository
{
    public class RepoDbContext: AppDbContext
    {
        public RepoDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            TablePrefix = "CF_Repo_";
        }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}

		public DbSet<Form>? Forms { get; set; }
	}
}

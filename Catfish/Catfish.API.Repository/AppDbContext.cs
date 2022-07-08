
namespace Catfish.API.Repository
{
    public class AppDbContext: CatfishExtensions.AppDbContext
    {

		public AppDbContext(DbContextOptions<CatfishExtensions.AppDbContext> options)
			: base(options)
		{
			TablePrefix = "CF_Repo_";
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

		}
	}
}

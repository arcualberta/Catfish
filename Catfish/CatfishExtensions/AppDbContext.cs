
namespace CatfishExtensions
{
    public class AppDbContext: DbContext
    {

		public AppDbContext(DbContextOptions<AppDbContext> oprions)
			: base(oprions)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}

	}
}

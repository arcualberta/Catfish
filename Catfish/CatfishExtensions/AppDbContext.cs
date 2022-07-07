
namespace CatfishExtensions
{
    public class AppDbContext: DbContext
    {
		protected string? TablePrefix;

		public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

            if (!string.IsNullOrEmpty(TablePrefix))
            {
                foreach (var entity in builder.Model.GetEntityTypes())
                    entity.SetTableName(TablePrefix + entity.GetTableName());
            }
        }

	}
}

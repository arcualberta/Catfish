namespace CatfishExtensions.Helpers
{
    public static class ConfigHelper
    {
        public static IConfiguration Configuration;
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static string SiteUrl
        {
            get
            {
                string val = Configuration.GetSection("SiteConfig:SiteUrl").Value;
                return string.IsNullOrEmpty(val) ? "" : val.TrimEnd('/');
            }
        }
    }
}

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

        public static string GetAttachmentsFolder(bool createIfNotExist = false)
        {
            string path = Path.Combine(Configuration.GetSection("SiteConfig:UploadRoot").Value, "attachments"); ;

            if (createIfNotExist)
                Directory.CreateDirectory(path);

            return path;
        }
    }
}

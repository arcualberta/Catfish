namespace Catfish.Web.Extensions
{
    public class CatfishWebModule : IModule
    {
        public string Author => "Arts Resource Centre, University of Alberta";

        public string Name => "Catfish Web Extensions";

        public string Version => Piranha.Utils.GetAssemblyVersion(GetType().Assembly);

        public string Description => "Catfish Extensions for Piranha CMS";

        public string PackageUrl => "https://arc.arts.ualberta.ca";

        public string IconUrl => "https://arc.arts.ualberta.ca";

        public void Init()
        {
            
        }
    }
}
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CatfishWebExtensions.Services
{
    public class AssetRegistry: IAssetRegistry
    {
        private readonly List<string> _stylesheets = new List<string>();
        private readonly List<string> _scripts = new List<string>();
        private readonly List<string> _modules = new List<string>();
        private bool _isDevEnvironment;
        private string _siteRoot;

        public AssetRegistry(IWebHostEnvironment env)
        {
            _isDevEnvironment = env.IsDevelopment();
            _siteRoot = ConfigHelper.SiteUrl;
        }
        public IReadOnlyList<string> GetStylesheets() => _stylesheets.AsReadOnly();
        public IReadOnlyList<string> GetScripts() => _scripts.AsReadOnly();
        public IReadOnlyList<string> GetModules() => _modules.AsReadOnly();


        public void RegisterStylesheet(string productionVersionPathName, string devVersionPathName = null)
        {
            var pathName = Path(productionVersionPathName, devVersionPathName);

            if (!_stylesheets.Contains(pathName))
                _stylesheets.Add(pathName);
        }

        public void RegisterScript(string productionVersionPathName, string devVersionPathName = null)
        {
            var pathName = Path(productionVersionPathName, devVersionPathName);

            if (!_scripts.Contains(pathName))
                _scripts.Add(pathName);
        }

        public void RegisterModule(string productionVersionPathName, string devVersionPathName = null)
        {
            var pathName = Path(productionVersionPathName, devVersionPathName);

            if (!_modules.Contains(pathName))
                _modules.Add(pathName);
        }

        private string Path(string productionVersionPathName, string devVersionPathName = null)
        {
            string pathName = (_isDevEnvironment && devVersionPathName != null)
                ? devVersionPathName
                : productionVersionPathName;

            if (!pathName.StartsWith("http"))
                pathName = _siteRoot + "/" + pathName;

            return pathName;
        }
    }
}

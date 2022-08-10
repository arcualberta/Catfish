using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CatfishWebExtensions.Services
{
    public class AssetRegistry: IAssetRegistry
    {
        private readonly List<string> _stylesheets = new List<string>();
        private readonly List<string> _scripts = new List<string>();
        private bool _isDevEnvironment;

        public AssetRegistry(IWebHostEnvironment env)
        {
            _isDevEnvironment = env.IsDevelopment();
        }
        public void RegisterStylesheet(string pathName)
        {
            if (!_stylesheets.Contains(pathName))
                _stylesheets.Add(pathName);
        }
        public IReadOnlyList<string> GetStylesheets()
        {
            return _stylesheets.AsReadOnly();
        }

        public void RegisterScript(string pathName)
        {
            if (!_scripts.Contains(pathName))
                _scripts.Add(pathName);
        }
        public IReadOnlyList<string> GetScripts()
        {
            return _scripts.AsReadOnly();
        }
    }
}

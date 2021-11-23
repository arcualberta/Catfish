using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Services
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
        public void RegisterStylesheet(string devPathName, string prodPathName)
        {
            string pathName = _isDevEnvironment ? devPathName : prodPathName;
            if (!_stylesheets.Contains(pathName))
                _stylesheets.Add(pathName);
        }
        public IReadOnlyList<string> GetStylesheets()
        {
            return _stylesheets.AsReadOnly();
        }

        public void RegisterScript(string devPathName, string prodPathName)
        {
            string pathName = _isDevEnvironment ? devPathName : prodPathName;
            if (!_scripts.Contains(pathName))
                _scripts.Add(pathName);
        }
        public IReadOnlyList<string> GetScripts()
        {
            return _scripts.AsReadOnly();
        }
    }
}

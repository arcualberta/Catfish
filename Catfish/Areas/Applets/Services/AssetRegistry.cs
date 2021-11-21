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

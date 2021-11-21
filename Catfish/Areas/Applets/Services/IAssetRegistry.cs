using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Services
{
    interface IAssetRegistry
    {
        public void RegisterStylesheet(string pathName);
        public IReadOnlyList<string> GetStylesheets();
        public void RegisterScript(string pathName);
        public IReadOnlyList<string> GetScripts();
    }
}

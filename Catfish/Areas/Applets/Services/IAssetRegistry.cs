using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Applets.Services
{
    public interface IAssetRegistry
    {
        public void RegisterStylesheet(string devPathName, string prodPathName);
        public IReadOnlyList<string> GetStylesheets();
        public void RegisterScript(string devPathName, string prodPathName);
        public IReadOnlyList<string> GetScripts();
    }
}

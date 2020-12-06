using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public interface IAppService
    {
        public void RegisterScript(string url);
        public void RegisterStylesheet(string url);

        public List<string> GetScriptUrls();
        public List<string> GetStylesheetUrls();
    }
}

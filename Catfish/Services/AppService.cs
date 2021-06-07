using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Services
{
    public class AppService : IAppService
    {
        private List<string> ScriptUrls = new List<string>();
        private List<string> StylesheetUrls = new List<string>();
        private List<string> PageLoadFunctions = new List<string>();

        public List<string> GetScriptUrls()
        {
            return ScriptUrls;
        }

        public List<string> GetStylesheetUrls()
        {
            return StylesheetUrls;
        }

        public List<string> GetOnLoadFunctionCalls()
        {
            return PageLoadFunctions;
        }
        public void RegisterScript(string url)
        {
            if (!ScriptUrls.Contains(url))
                ScriptUrls.Add(url);
        }

        public void RegisterStylesheet(string url)
        {
            if (!StylesheetUrls.Contains(url))
                StylesheetUrls.Add(url);
        }

        public void RegisterOnLoadFunction(string fucntionCall)
        {
            if (!fucntionCall.EndsWith(";"))
                fucntionCall = fucntionCall + ";";

            if (!PageLoadFunctions.Contains(fucntionCall))
                PageLoadFunctions.Add(fucntionCall);
        }
    }
}

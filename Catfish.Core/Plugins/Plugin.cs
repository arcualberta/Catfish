using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Catfish.Core.Plugins
{
    public abstract class Plugin
    {
        public string BasePath { get; set; }

        public abstract void Initialize();

        public virtual void RegisterRoutes(RouteCollection routes)
        {

        }

        protected void CopyDirectory(string sourceFolder, string destinationFolder, bool overwrite = false)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceFolder);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory not found: " + sourceFolder);
            }

            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            // Copy files
            FileInfo[] files = dir.GetFiles();
            foreach(FileInfo file in files)
            {
                string resultPath = Path.Combine(destinationFolder, file.Name);
                file.CopyTo(resultPath, overwrite);
            }

            // Recursivly copy folders
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach(DirectoryInfo childDir in dirs)
            {
                string resultPath = Path.Combine(destinationFolder, childDir.Name);
                CopyDirectory(childDir.FullName, resultPath, overwrite);
            }
        }

        public virtual string GetPluginPath()
        {
            if(BasePath != null)
            {
                return BasePath;
            }

            Assembly pluginAsm = GetType().Assembly;
            string pluginPath = pluginAsm.Location.Substring(0, pluginAsm.Location.LastIndexOf(pluginAsm.ManifestModule.Name));

            return pluginPath;
        }
        
        public virtual void CopyBaseViews(string destinationFolder)
        {
            CopyDirectory(Path.Combine(GetPluginPath(), "Views"), destinationFolder, true);
        }

        public virtual void CopyManagerViews(string destinationFolder)
        {
            CopyDirectory(Path.Combine(GetPluginPath(), "Areas/Manager/Views"), destinationFolder, true);
        }
    }
}

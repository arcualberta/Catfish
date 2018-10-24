using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Catfish.Core.Plugins
{
    public abstract class Plugin
    {
        public abstract void Initialize();

        public virtual void RegisterRoutes(RouteCollection routes)
        {

        }

        protected string GetVirtualPath()
        {
            System.Reflection.Assembly asm = this.GetType().Assembly;
            string basePath = asm.Location.Substring(0, asm.Location.LastIndexOf(asm.ManifestModule.Name));

            Uri path1 = new Uri(System.AppContext.BaseDirectory);
            Uri path2 = new Uri(basePath);

            return "~/" + path1.MakeRelativeUri(path2).OriginalString;
        }

        public virtual string GetAreaViewsPath()
        {
            string basePath = GetVirtualPath();

            return string.Format("{0}/{1}/Manager/Views/", basePath, "{2}");
        }

        public virtual string GetViewsPath()
        {
            string basePath = GetVirtualPath();

            return string.Format("{0}/Views/", basePath);
        }
    }
}

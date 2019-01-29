using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Plugins
{
    public class PluginContext
    {
        private static PluginContext mCurrent;
        public static PluginContext Current
        {
            get
            {
                if(mCurrent == null)
                {
                    mCurrent = new PluginContext();
                }

                return mCurrent;
            }
        }

        private List<Assembly> mAssemblies;

        private List<Plugin> mPlugins;
        public IReadOnlyList<Plugin> Plugins
        {
            get
            {
                return mPlugins.AsReadOnly();
            }
        }

        public PluginViewEngine ViewEngine { get; private set; }

        private PluginContext()
        {
            mPlugins = new List<Plugin>();
            mAssemblies = new List<Assembly>();
            ViewEngine = new PluginViewEngine();
        }

        public Plugin LoadPlugin(string pluginClass, string assemblyLocation, string basePath)
        {
            AssemblyName name = AssemblyName.GetAssemblyName(assemblyLocation);
            Assembly asm = Assembly.Load(name);

            Plugin result = asm.CreateInstance(pluginClass) as Plugin;
            result.BasePath = basePath;
            
            mPlugins.Add(result);

            if (!mAssemblies.Contains(asm))
            {
                mAssemblies.Add(asm);
                ViewEngine.RegisterPlugin(result);
            }

            return result;
        }

        public ICollection<Type> GetTypes(IEnumerable<string> namespaces, string className)
        {
            IEnumerable<string> check = namespaces.Select(n => n + "." + className);
            List<Type> result = new List<Type>();

            foreach(Assembly asm in mAssemblies)
            {
                foreach(string name in check)
                {
                    Type type = asm.GetType(name);

                    if(type != null)
                    {
                        result.Add(type);
                    }
                }
            }

            return result.AsReadOnly();
        }
    }
}

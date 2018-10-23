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

        private List<Plugin> mPlugins;
        public IReadOnlyList<Plugin> Plugins
        {
            get
            {
                return mPlugins.AsReadOnly();
            }
        }

        private PluginContext()
        {
            mPlugins = new List<Plugin>();
        }

        public Plugin LoadPlugin(string pluginClass, string assemblyLocation)
        {
            AssemblyName name = AssemblyName.GetAssemblyName(assemblyLocation);
            Assembly asm = Assembly.Load(name);

            Plugin result = asm.CreateInstance(pluginClass) as Plugin;

            mPlugins.Add(result);

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catfish.Core.Plugins
{
    public class PluginConfig : ConfigurationSection
    {
        [ConfigurationProperty("plugins")]
        [ConfigurationCollection(typeof(PluginElementCollection),
            AddItemName = "plugin")]
        public PluginElementCollection Plugins
        {
            get { return this["plugins"] as PluginElementCollection;  }
            set { this["plugins"] = value; }
        }

        public PluginConfig()
        {
            //Plugins = new PluginElementCollection();
        }
    }

    public class PluginElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new PluginElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as PluginElement).Class;
        }
    }

    public class PluginElement : ConfigurationElement
    {
        [ConfigurationProperty("class", IsRequired = true)]
        public String Class
        {
            get
            {
                return (string)this["class"];
            }

            set
            {
                this["class"] = value;
            }
        }

        [ConfigurationProperty("libraryPath", IsRequired = true)]
        public String LibraryPath
        {
            get
            {
                return (string)this["libraryPath"];
            }

            set
            {
                this["libraryPath"] = value;
            }
        }
    }
}

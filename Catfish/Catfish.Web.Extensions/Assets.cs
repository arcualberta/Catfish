using CatfishWebExtensions.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CatfishWebExtensions
{
    public static class Assets
    {
        public static List<PartialView> Headers { get; private set; } = new List<PartialView>();
        public static List<PartialView> Footers { get; private set; } = new List<PartialView>();

        public static IEnumerable<HeaderModelAttribute> GetHeaderTypes()
        {
            Type t = typeof(HeaderModelAttribute);
            Assembly assemFromType = t.Assembly;

            List<HeaderModelAttribute> ret = new List<HeaderModelAttribute>();

            foreach (Type type in assemFromType.GetTypes())
            {
                foreach(var att in type.GetCustomAttributes(typeof(HeaderModelAttribute), true))
                {
                    ret.Add(att as HeaderModelAttribute);
                }
            }

            return ret;
        }
        public static IEnumerable<FooterModelAttribute> GetFooterTypes()
        {
            Type t = typeof(FooterModelAttribute);
            Assembly assemFromType = t.Assembly;

            List<FooterModelAttribute> ret = new List<FooterModelAttribute>();

            foreach (Type type in assemFromType.GetTypes())
            {
                foreach (var att in type.GetCustomAttributes(typeof(FooterModelAttribute), true))
                {
                    ret.Add(att as FooterModelAttribute);
                }
            }

            return ret;
        }


    }

    public class PartialView
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public PartialView() { }
        public PartialView(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}

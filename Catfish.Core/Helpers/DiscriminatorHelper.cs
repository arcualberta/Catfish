using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Catfish.Core.Helpers
{
    public static class DiscriminatorHelper
    {
        //public static IEnumerable<Type> GetAllDiscriminatorType(Type p)
        //{
        //    foreach( var ass in AppDomain.CurrentDomain.GetAssemblies())
        //    {
        //        foreach(var t in ass.GetTypes())
        //        {
        //            if (t.IsSubclassOf(p))
        //                yield return t;

        //        }
        //    }
        //}

        public static List<string> GetDiscriminatorWhere<T>()
        {
            List<string> allDiscriminators = new List<string>();
            var subclassTypes = Assembly.GetAssembly(typeof(T)).GetTypes().Where(t => t.IsSubclassOf(typeof(T)));
            foreach(var s in subclassTypes)
            {
                allDiscriminators.Add(s.Name);
            }
            return allDiscriminators;
        }

       
    }
}

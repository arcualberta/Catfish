using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Access
{
    public abstract class CatfishSecurity
    {
        public abstract void BuildPolicies(IServiceCollection services);

        /// <summary>
        /// Overrides of this method are expected to define how custom permissions should 
        /// appear under permission-management screen for various roles. 
        /// </summary>
        public abstract void AddPermissions();

        public static void BuildAllPolicies(IServiceCollection services)
        {
            var implementations = GetSecurityImplementations();

            //Instantiating each subclass type and calling their initialzie method
            foreach (var instance in implementations)
                instance.BuildPolicies(services);
        }

        public static void AddPermissionEntriesToApp()
        {
            var implementations = GetSecurityImplementations();

            //Instantiating each subclass type and calling their initialzie method
            foreach (var instance in implementations)
                instance.AddPermissions();
        }

        protected static List<CatfishSecurity> GetSecurityImplementations()
        {
            return Assembly.GetAssembly(typeof(CatfishSecurity))
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(CatfishSecurity)))
                .Select(t => Activator.CreateInstance(t) as CatfishSecurity)
                .ToList();
        }
    }
}

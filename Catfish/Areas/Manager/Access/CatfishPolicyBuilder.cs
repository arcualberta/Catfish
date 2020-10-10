using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Access
{
    public abstract class CatfishPolicyBuilder
    {
        public abstract void Initialize(IServiceCollection services);

        public static void InitAll(IServiceCollection services)
        {
            var subclasses = Assembly.GetAssembly(typeof(CatfishPolicyBuilder)).GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(CatfishPolicyBuilder)));

            //Instantiating each subclass type and calling their initialzie method
            foreach(var subclass in subclasses)
            {
                var policyBuilder = Activator.CreateInstance(subclass) as CatfishPolicyBuilder;
                policyBuilder.Initialize(services);
            }
        }
    }
}

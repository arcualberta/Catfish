using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Access
{
    public abstract class AbstractCatfishPolicy
    {
        public abstract void Build(IServiceCollection services);

        public static void BuildAllPolicies(IServiceCollection services)
        {
            var subclasses = Assembly.GetAssembly(typeof(AbstractCatfishPolicy)).GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(AbstractCatfishPolicy)));

            //Instantiating each subclass type and calling their initialzie method
            foreach(var subclass in subclasses)
            {
                var policyBuilder = Activator.CreateInstance(subclass) as AbstractCatfishPolicy;
                policyBuilder.Build(services);
            }
        }
    }
}

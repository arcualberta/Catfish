using CatfishExtensions.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatfishExtensions.Helpers
{
    internal static class CorsHelper
    {
        public static void AddPolicies(ConfigurationManager configuration, IServiceCollection services)
        {
            string generalHosts = configuration.GetSection("CatfishAllowedHosts:General").Value;
            string restrictedHosts = configuration.GetSection("CatfishAllowedHosts:Restricted").Value;

            AddPolicy(CorsPolicyNames.General, generalHosts, services);
            AddPolicy(CorsPolicyNames.Restricted, restrictedHosts, services);
        }

        private static void AddPolicy(string policyName, string allowedHosts, IServiceCollection services)
        {
            if (allowedHosts == "*")
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(policyName,
                                    builder =>
                                    {
                                        builder.AllowAnyOrigin()
                                               .AllowAnyHeader()
                                               .AllowAnyMethod();
                                    });
                });
            }
            else if(!string.IsNullOrWhiteSpace(allowedHosts))
            {
                var hosts = allowedHosts.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                services.AddCors(options =>
                {
                    options.AddPolicy(policyName,
                                    builder =>
                                    {
                                        builder.WithOrigins(hosts)
                                               .AllowAnyHeader()
                                               .AllowAnyMethod();
                                    });
                });
            }
        }

    }
}

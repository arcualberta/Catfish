using Microsoft.Extensions.DependencyInjection;
using Piranha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Access
{
    public class ProcessSecurity : CatfishSecurity
    {
        public static readonly string PageAccess = "ProcessesPageAccess";
        public static readonly string Reindex = "ContentReindex";

        public override void AddPermissions()
        {
            App.Permissions["Manager"].Add(new Piranha.Security.PermissionItem
            {
                Category = "Processes",
                Title = "Processes Page Access",
                Name = PageAccess
            });

            App.Permissions["Manager"].Add(new Piranha.Security.PermissionItem
            {
                Category = "Processes",
                Title = "Reindex",
                Name = Reindex
            });
        }

        public override void BuildPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options => {
                options.AddPolicy(PageAccess, x => x.RequireClaim(PageAccess));
            });
            services.AddAuthorization(options => {
                options.AddPolicy(Reindex, x => x.RequireClaim(Reindex));
            });
        }
    }
}

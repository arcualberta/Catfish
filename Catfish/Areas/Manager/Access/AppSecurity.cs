using Microsoft.Extensions.DependencyInjection;
using Piranha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Access
{
    public class AppSecurity : CatfishSecurity
    {
        public static readonly string ReadSecurePages = "ReadSecurePages";
        public override void AddPermissions()
        {
            App.Permissions["App"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Read Secure Pages",
                Name = ReadSecurePages
            });
        }

        public override void BuildPolicies(IServiceCollection services)
        {
            services.AddAuthorization(o =>
            { //read secure posts
                o.AddPolicy("ReadSecurePosts", policy => {
                    policy.RequireClaim("ReadSecurePosts", "ReadSecurePosts");
                });
            });
        }
    }
}

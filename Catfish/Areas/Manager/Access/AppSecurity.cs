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
        public static readonly string EditTheme = "EditTheme";
        public static readonly string AccessEntities = "AccessEntities";
        public static readonly string AccessTemplates = "AccessTemplates";
        public override void AddPermissions()
        {
            App.Permissions["App"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Read Secure Pages",
                Name = ReadSecurePages
            });

            App.Permissions["App"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Edit Theme",
                Name = EditTheme
            });

            App.Permissions["App"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Access Entities",
                Name = AccessEntities
            });

            App.Permissions["App"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Access Templates",
                Name = AccessTemplates
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

            services.AddAuthorization(o =>
            { //read secure posts
                o.AddPolicy("EditTheme", policy => {
                    policy.RequireClaim("EditTheme", "EditTheme");
                });
            });

            services.AddAuthorization(o =>
            { //read secure posts
                o.AddPolicy("AccessEntities", policy => {
                    policy.RequireClaim("AccessEntities", "AccessEntities");
                });
            });

            services.AddAuthorization(o =>
            { //read secure posts
                o.AddPolicy("AccessTemplates", policy => {
                    policy.RequireClaim("AccessTemplates", "AccessTemplates");
                });
            });

        }
    }
}

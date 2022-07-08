using Microsoft.Extensions.DependencyInjection;
using Piranha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Access
{
    public class DataSecurity : CatfishSecurity
    {
        public static readonly string CreateSubmission = "CreateSubmission";
        public override void AddPermissions()
        {
            App.Permissions["Workflow"].Add(new Piranha.Security.PermissionItem
            {
                Category = "Submissions",
                Title = "Create Submissions",
                Name = CreateSubmission
            });

        }

        public override void BuildPolicies(IServiceCollection services)
        {
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CreateEntityPolicy",
            //      policy => policy.RequireClaim("Create Submission"));
            //});

            //services.AddAuthorization(o =>
            //{
            //    o.AddPolicy("CreateSubmission", policy => {
            //        policy.RequireClaim("CreateSubmission", "CreateSubmission");
            //    });
            //});

            //services.AddAuthorization(o =>
            //{
            //    o.AddPolicy("CreateSubmission", policy => {
            //        policy.RequireClaim("CreateSubmission", "CreateSubmission");
            //    });
            //});
        }
    }
}

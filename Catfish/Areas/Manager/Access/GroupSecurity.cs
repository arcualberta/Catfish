using Microsoft.Extensions.DependencyInjection;
using Piranha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Access
{
    public class GroupSecurity : CatfishSecurity
    {
        public static readonly string List = "GroupsList";
        public static readonly string Add = "GroupsAdd";
        public static readonly string Edit = "GroupsEdit";
        public static readonly string Save = "GroupsSave";
        public static readonly string Delete = "GroupsDelete";

        public override void AddPermissions()
        {
            App.Permissions["Workflow"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Add Groups",
                Name = "GroupsAdd",
                Category = "Groups"
            });

            App.Permissions["Workflow"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Edit Groups",
                Name = "GroupsEdit",
                Category = "Groups"
            });
            App.Permissions["Workflow"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Save Groups",
                Name = "GroupsSave",
                Category = "Groups"
            });
            App.Permissions["Workflow"].Add(new Piranha.Security.PermissionItem
            {
                Title = "Delete Groups",
                Name = "GroupsDelete",
                Category = "Groups"
            });
            App.Permissions["Workflow"].Add(new Piranha.Security.PermissionItem
            {
                Title = "List Group",
                Name = "GroupsList",
                Category = "Groups"
            });
        }

        public override void BuildPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options => {
                options.AddPolicy(List, x => x.RequireClaim(List)); 
            });
            services.AddAuthorization(options => {
                options.AddPolicy(Add, x => x.RequireClaim(Add));
            });
            services.AddAuthorization(options => {
                options.AddPolicy(Edit, x => x.RequireClaim(Edit));
            });
            services.AddAuthorization(options => {
                options.AddPolicy(Save, x => x.RequireClaim(Save));
            });
            services.AddAuthorization(options => {
                options.AddPolicy(Delete, x => x.RequireClaim(Delete));
            });
        }
    }
}

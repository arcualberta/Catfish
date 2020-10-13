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
        public static readonly string PageAccess = "GroupsPageAccess";
        public static readonly string Add = "GroupsAdd";
        public static readonly string Edit = "GroupsEdit";
        public static readonly string Save = "GroupsSave";
        public static readonly string Delete = "GroupsDelete";

        public override void AddPermissions()
        {
            App.Permissions["Workflow"].Add(new Piranha.Security.PermissionItem
            {
                Category = "Groups",
                Title = "Add Groups",
                Name = Add
            });

            App.Permissions["Workflow"].Add(new Piranha.Security.PermissionItem
            {
                Category = "Groups",
                Title = "Edit Groups",
                Name = Edit
            });
            App.Permissions["Workflow"].Add(new Piranha.Security.PermissionItem
            {
                Category = "Groups",
                Title = "Save Groups",
                Name = Save
            });
            App.Permissions["Workflow"].Add(new Piranha.Security.PermissionItem
            {
                Category = "Groups",
                Title = "Delete Groups",
                Name = Delete
            });
            App.Permissions["Workflow"].Add(new Piranha.Security.PermissionItem
            {
                Category = "Groups",
                Title = "Access Groups Page",
                Name = PageAccess
            });
        }

        public override void BuildPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options => {
                options.AddPolicy(PageAccess, x => x.RequireClaim(PageAccess)); 
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

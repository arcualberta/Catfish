using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Access
{
    public class GroupPolicy : AbstractCatfishPolicy
    {
        public static readonly string List = "GroupsList";
        public static readonly string Add = "GroupsAdd";
        public static readonly string Edit = "GroupsEdit";
        public static readonly string Save = "GroupsSave";
        public static readonly string Delete = "GroupsDelete";

        public override void Build(IServiceCollection services)
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

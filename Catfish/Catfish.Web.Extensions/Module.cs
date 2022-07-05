using Piranha;
using Piranha.Extend;
using Piranha.Manager;
using Piranha.Security;

namespace CatfishWebExtensions
{
    public class Module : IModule
    {
        private readonly List<PermissionItem> _permissions = new List<PermissionItem>
        {
            new PermissionItem { Name = Permissions.CatfishWebExtensions, Title = "List CatfishWebExtensions content", Category = "CatfishWebExtensions", IsInternal = true },
            new PermissionItem { Name = Permissions.CatfishWebExtensionsAdd, Title = "Add CatfishWebExtensions content", Category = "CatfishWebExtensions", IsInternal = true },
            new PermissionItem { Name = Permissions.CatfishWebExtensionsEdit, Title = "Edit CatfishWebExtensions content", Category = "CatfishWebExtensions", IsInternal = true },
            new PermissionItem { Name = Permissions.CatfishWebExtensionsDelete, Title = "Delete CatfishWebExtensions content", Category = "CatfishWebExtensions", IsInternal = true }
        };

        /// <summary>
        /// Gets the module author
        /// </summary>
        public string Author => "Arts Resource Centre";

        /// <summary>
        /// Gets the module name
        /// </summary>
        public string Name => "CatfishWebExtensions";

        /// <summary>
        /// Gets the module version
        /// </summary>
        public string Version => Utils.GetAssemblyVersion(GetType().Assembly);

        /// <summary>
        /// Gets the module description
        /// </summary>
        public string Description => "";

        /// <summary>
        /// Gets the module package url
        /// </summary>
        public string PackageUrl => "";

        /// <summary>
        /// Gets the module icon url
        /// </summary>
        public string IconUrl => "/manager/PiranhaModule/piranha-logo.png";

        public void Init()
        {
            // Register permissions
            foreach (var permission in _permissions)
            {
                App.Permissions["CatfishWebExtensions"].Add(permission);
            }

            // Add manager menu items
            Menu.Items.Add(new MenuItem
            {
                InternalId = "CatfishWebExtensions",
                Name = "CatfishWebExtensions",
                Css = "fas fa-box"
            });
            Menu.Items["CatfishWebExtensions"].Items.Add(new MenuItem
            {
                InternalId = "CatfishWebExtensionsStart",
                Name = "Module Start",
                Route = "~/manager/catfishwebextensions",
                Policy = Permissions.CatfishWebExtensions,
                Css = "fas fa-box"
            });
        }
    }
}

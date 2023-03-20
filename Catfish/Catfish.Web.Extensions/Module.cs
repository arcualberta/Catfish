using Piranha;
using Piranha.Extend;
using Piranha.Manager;
using Piranha.Security;

using CatfishWebExtensions.Models.Blocks;

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
        public string IconUrl => "/manager/images/arc.png";

        public void Init()
        {

            // Register permissions
            foreach (var permission in _permissions)
            {
                App.Permissions["CatfishWebExtensions"].Add(permission);
            }

            AddRepositoryExtensions();
            AddBlockExtensions();

        }

        private void AddRepositoryExtensions()
        {
            MenuItem group;

            group = Menu.Items.FirstOrDefault(ite => ite.Name == "Content");
            group.Items.Add(new MenuItem
            {
                InternalId = "CatfishWebExtensionsStyleSheets",
                Name = "StyleSheets",
                Route = "~/manager/styleSheets",
                //Policy = Permissions.CatfishWebExtensions,
                Css = "fab fa-css3"
            });

            ////// Add manager menu items
            ////Menu.Items.Add(new MenuItem
            ////{
            ////    InternalId = "CatfishWebExtensions",
            ////    Name = "CatfishWebExtensions",
            ////    Css = "fas fa-box"
            ////});
            ////Menu.Items["CatfishWebExtensions"].Items.Add(new MenuItem
            ////{
            ////    InternalId = "CatfishWebExtensionsStart",
            ////    Name = "Module Start",
            ////    Route = "~/manager/catfishwebextensions",
            ////    Policy = Permissions.CatfishWebExtensions,
            ////    Css = "fas fa-box"
            ////});

            //Renaming the "Content" group as "Web"
            group = Menu.Items.FirstOrDefault(ite => ite.Name == "Content");
            if (group != null)
                group.Name = "Web";

            //Inserting the "Data" group as the second group in the dashboard's left menu.
            Menu.Items.Insert(1, group = new MenuItem
            {
                InternalId = "CatfishWebExtensionsData",
                Name = "Data",
                Css = "fas fa-box"
            });
            group.Items.Add(new MenuItem
            {
                InternalId = "CatfishWebExtensionstems",
                Name = "Items",
                Route = "~/manager/items",
                //Policy = Permissions.CatfishWebExtensions,
                Css = "fas fa-chess"
            });
            group.Items.Add(new MenuItem
            {
                InternalId = "CatfishWebExtensionsCollections",
                Name = "Collections",
                Route = "~/manager/collections",
                //Policy = Permissions.CatfishWebExtensions,
                Css = "fas fa-cubes"
            });

            //Inserting the "Templates" group
            Menu.Items.Insert(2, group = new MenuItem
            {
                InternalId = "CatfishWebExtensionsTemplates",
                Name = "Templates",
                Css = "fas fa-toolbox"
            });
            group.Items.Add(new MenuItem
            {
                InternalId = "CatfishWebExtensionsForms",
                Name = "Forms",
                Route = "~/manager/forms",
                //Policy = Permissions.CatfishWebExtensions,
                Css = "fab fa-wpforms"
            });
            group.Items.Add(new MenuItem
            {
                InternalId = "CatfishWebExtensionsTemplates",
                Name = "Templates",
                Route = "~/manager/templates",
                //Policy = Permissions.CatfishWebExtensions,
                Css = "fas fa-stroopwafel"
            });
            group.Items.Add(new MenuItem
            {
                InternalId = "CatfishWebExtensionsWorkflows",
                Name = "Workflows",
                Route = "~/manager/workflows",
                //Policy = Permissions.CatfishWebExtensions,
                Css = "fas fa-project-diagram"
            });

            //Removing the Settings group
            group = Menu.Items.FirstOrDefault(ite => ite.Name == "Settings");
            Menu.Items.Remove(group);


        }

        private void AddBlockExtensions()
        {
            App.Blocks.Register<Css>();
        }
    }
}

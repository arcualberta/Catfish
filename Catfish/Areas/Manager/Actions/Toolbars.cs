using Piranha.Manager.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Actions
{
    public static class Toolbars
    {
        /// <summary>
        /// Gets the actions available for the page list view.
        /// </summary>
        public static ActionList<ToolbarAction> EntityTypeList { get; private set; } = new ActionList<ToolbarAction>
            {
                new ToolbarAction
                {
                    InternalId = "AddEntityType",
                    ActionView = "Partial/Actions/_EntityTypeListAddEntityType"

                }
            };

        /// <summary>
        /// Gets the actions available for the ItemList view.
        /// </summary>
        public static ActionList<ToolbarAction> CollectionList { get; private set; } = new ActionList<ToolbarAction>
            {

                new ToolbarAction
                {
                    InternalId = "AddCollection",
                    ActionView = "Partial/Actions/_ItemListAddCollection"

                }

            };

        public static ActionList<ToolbarAction> CollapseExpandList { get; private set; } = new ActionList<ToolbarAction>
        {
                new ToolbarAction
                {
                    InternalId = "CollapseExpand",
                    ActionView = "Partial/Actions/_ItemListCollapseExpand"

                }

        };
    }
}

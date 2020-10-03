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


        /// <summary>
        /// Gets the actions available for the ItemList view.
        /// </summary>
        public static ActionList<ToolbarAction> FormList { get; private set; } = new ActionList<ToolbarAction>
            {

                new ToolbarAction
                {
                    InternalId = "AddForm",
                    ActionView = "Partial/Actions/_FormListAddForm"

                }

            };

        /// <summary>
        /// Gets the actions available for collapse all/expand all in ItemList view. 
        /// It is apart from the actions because they are not located in the same section.
        /// </summary>
        public static ActionList<ToolbarAction> CollapseExpandList { get; private set; } = new ActionList<ToolbarAction>
        {
                new ToolbarAction
                {
                    InternalId = "CollapseExpand",
                    ActionView = "Partial/Actions/_ItemListCollapseExpand"

                }

        };


        /// <summary>
        /// Gets the actions available for collapse all/expand all in ItemList view. 
        /// It is apart from the actions because they are not located in the same section.
        /// </summary>
        public static ActionList<ToolbarAction> SavePreviewEditItem { get; private set; } = new ActionList<ToolbarAction>
        {
                new ToolbarAction
                {
                    InternalId = "SavePreviewEditItem",
                    ActionView = "Partial/Actions/_SavePreviewEditItem"
                }

        };


        /// <summary>
        /// Saves the field form in the FieldContainerEdit view. 
        /// </summary>
        public static ActionList<ToolbarAction> SaveFieldForm { get; private set; } = new ActionList<ToolbarAction>
        {
                new ToolbarAction
                {
                    InternalId = "SaveFieldForm",
                    ActionView = "Partial/Actions/_SaveFieldForm"
                }

        };
    }
}

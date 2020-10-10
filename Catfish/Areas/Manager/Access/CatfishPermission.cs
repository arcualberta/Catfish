using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catfish.Areas.Manager.Access
{
    public static class CatfishPermission
    {
        //Group Management Permissions
        public const string Group = "Groups";
        public const string GroupsSave = "GroupsSave";
        public const string GroupsList = "GroupsList";
        public const string GroupsEdit = "GroupsEdit";
        public const string GroupsDelete = "GroupsDelete";
        public const string GroupsAdd = "GroupsAdd";

        //Process Management Permissions
        public const string ManageProcesses = "ManageProcesses";
    }
    //public static string[] All();

}

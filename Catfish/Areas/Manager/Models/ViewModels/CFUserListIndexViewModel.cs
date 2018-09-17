using Catfish.Core.Models;
using Catfish.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class CFUserListIndexViewModel
    {
        public Guid Guid { get; }
        public string UserListName { get; }

        private List<Piranha.Entities.User> Users { get; set; }
        private UserService userService = new UserService();

        public CFUserListIndexViewModel()
        {
            Guid = new Guid();
            UserListName = "";
            Users = new List<Piranha.Entities.User>();
        }

        public CFUserListIndexViewModel(CFUserList userList)
        {
            Guid = userList.Id;
            UserListName = userList.Name;
            Users = new List<Piranha.Entities.User>();
            foreach ( CFUserListEntry userListEntry in userList.CFUserListEntries)
            {
                string userId = userListEntry.UserId.ToString();
                Users.Add(userService.GetUserById(userId));
            }
            
        }

        public string[] UserNames()
        {
            string[] result = new string[Users.Count];
            for (int i =0; i< Users.Count; ++i)
            {
                result[i] = Users[i].Login;
            }

            return result;
        }
    }
}
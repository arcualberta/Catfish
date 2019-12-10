using Catfish.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Catfish.Areas.Manager.Models.ViewModels
{
    public class CFUserListViewModel
    {
        public string Id { get; set; }

        [Display(Name = "User List Name")]
        [Required(ErrorMessage = "This field is required.")]
        public string CFUserListName { get; set; }

        public string userName { get; set; } /*user's loginName to be added -- used on edit view */

        
        public List<string> SelectedUsers { get; set; }
       
       // public List<string> AllUsers { get; set; }
        public Dictionary<string, string> AllUsers2 { get; set; }

        public List<string> UsersToRemove { get; set; }

        public string ErrorMessage { get; set; }
        [NotMapped]
        public string SuccessMessage { get; set; }

        public CFUserListViewModel()
        {
            SelectedUsers = new List<string>();
            UsersToRemove = new List<string>(); 
            AllUsers2 = new Dictionary<string, string>();
        }

        public void AddUser()
        {
            if(!string.IsNullOrEmpty(userName) && !SelectedUsers.Contains(userName))//SelectedUsers.Where(u=>u.Login == userName).Count() <= 0)
            {
                if(AllUsers2.ContainsValue(userName))
                    SelectedUsers.Add(userName);
                userName = "";
            }
        }
        /// <summary>
        /// removed selected user 
        /// </summary>
        public void RemoveSelected()
        {
            SelectedUsers.RemoveAll(u => UsersToRemove.Contains(u));//(u => UsersToRemove.Any(y=>y.Id == u.Id));
            UsersToRemove.Clear();
        }

        public CFUserList UpdateModel(CFUserList entityGroup)
        {
            if (entityGroup == null)
            {
                entityGroup = new CFUserList();
                entityGroup.Id = Guid.NewGuid();
            }

            entityGroup.Name = CFUserListName;
            if (entityGroup.CFUserListEntries.Count > 0)
                entityGroup.CFUserListEntries.Clear();

            foreach (string usrLogin in SelectedUsers)
            {
                var _usr = AllUsers2.FirstOrDefault(u => u.Value == usrLogin);
                if(_usr.Key != null)
                {
                    CFUserListEntry egUser = new CFUserListEntry()
                    {
                        CFUserListId = entityGroup.Id,
                        UserId = Guid.Parse(_usr.Key)
                    };
                    entityGroup.CFUserListEntries.Add(egUser);
                }
            }

            return entityGroup;
        }

    }
}